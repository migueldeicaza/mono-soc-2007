using System;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Xml;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.template;
using umbraco.cms.businesslogic.web;
using umbraco.interfaces;

namespace umbraco
{
    /// <summary>
    /// Summary description for requestHandler.
    /// </summary>
    public class requestHandler
    {
        #region public properties

        public XmlNode currentPage;

        public String PageXPathQuery
        {
            get { return _pageXPathQuery; }
        }

        #endregion

        private static Hashtable _processedRequests = new Hashtable();
        private static string pageXPathQueryStart = "/root";
        private static string _urlName = string.Empty;
        private static bool _urlNameInitialized = false;
        private static XmlDocument _customHandlers;

        private string _pageXPathQuery = string.Empty;
        private bool _doNotCache = false;

        public static void ClearProcessedRequests()
        {
            _processedRequests.Clear();
        }

        public static string cleanUrl()
        {
            string tmp = HttpContext.Current.Request.Path.ToLower().Replace("/default.aspx", string.Empty);
            if (tmp != string.Empty)
                tmp = helper.Request("umbPage").ToLower().Replace(tmp, string.Empty);
            else
                tmp = helper.Request("umbPage").ToLower();

            if (tmp == "/default.aspx")
                tmp = string.Empty;
            else if (tmp == "/")
                tmp = string.Empty;

            return tmp;
        }

        // Init urlName to correspond to web.config entries (umbracoUrlForbittenCharacters and umbracoUrlSpaceCharacter).
        // Needed to compensate for known asp.net framework error KB826437:
        // http://support.microsoft.com/default.aspx?scid=kb;EN-US;826437
        private static void InitializeUrlName()
        {
            /*			string toReplace = string.Empty;
			string replaceWith = string.Empty;
			XmlNode replaceChars = UmbracoSettings.UrlReplaceCharacters;
			foreach (XmlNode n in replaceChars.SelectNodes("char")) 
			{
				if (xmlHelper.GetNodeValue(n).Trim() != string.Empty) 
				{
					toReplace += n.Attributes.GetNamedItem("org").Value;
					replaceWith += n.FirstChild.Value.Trim();
				}
			}
			_urlName = "translate(@urlName, '" + toReplace + "','" + replaceWith + "')";

*/
            _urlName = "@urlName";
            _urlNameInitialized = true;
        }

        public static string CreateXPathQuery(string url, bool checkDomain)
        {
            string _tempQuery = "";
            if (GlobalSettings.HideTopLevelNodeFromPath && checkDomain)
            {
                _tempQuery = "/root/node";
            }
            else if (checkDomain)
                _tempQuery = "/root";


            string[] requestRawUrl = url.Split("/".ToCharArray());

            // Check for Domain prefix
            string domainUrl = "";
            if (checkDomain && Domain.Exists(HttpContext.Current.Request.ServerVariables["SERVER_NAME"]))
            {
                domainUrl =
                    library.NiceUrlFullPath(
                        Domain.GetRootFromDomain(HttpContext.Current.Request.ServerVariables["SERVER_NAME"]));
                // If at domain root
                if (url == "")
                {
                    _tempQuery = "";
                    requestRawUrl = domainUrl.Split("/".ToCharArray());
                    HttpContext.Current.Trace.Write("requestHandler",
                                                    "Redirecting to domain: " +
                                                    HttpContext.Current.Request.ServerVariables["SERVER_NAME"] +
                                                    ", nodeId: " +
                                                    Domain.GetRootFromDomain(
                                                        HttpContext.Current.Request.ServerVariables["SERVER_NAME"]).
                                                        ToString());
                }
                else
                {
                    // if it matches a domain url, skip all other xpaths and use this!
                    string langXpath = CreateXPathQuery(domainUrl + "/" + url, false);
                    if (content.Instance.XmlContent.DocumentElement.SelectSingleNode(langXpath) != null)
                        return langXpath;
                    else if (UmbracoSettings.UseDomainPrefixes)
                        return "/domainprefixes-are-used-so-i-do-not-work";
                }
            }
            else if (url == "" && !GlobalSettings.HideTopLevelNodeFromPath)
                _tempQuery += "/node";

            bool rootAdded = false;
            if (GlobalSettings.HideTopLevelNodeFromPath && requestRawUrl.Length == 1)
            {
                HttpContext.Current.Trace.Write("umbracoRequestHandler", "xpath: '" + _tempQuery + "'");
                if (_tempQuery == "")
                    _tempQuery = "/root/node";
                _tempQuery = "/root/node [" + _urlName +
                             " = \"" + requestRawUrl[0].Replace(".aspx", "").ToLower() + "\"] | " + _tempQuery;
                HttpContext.Current.Trace.Write("umbracoRequestHandler", "xpath: '" + _tempQuery + "'");
                rootAdded = true;
            }


            for (int i = 0; i <= requestRawUrl.GetUpperBound(0); i++)
            {
                if (requestRawUrl[i] != "")
                    _tempQuery += "/node [" + _urlName + " = \"" + requestRawUrl[i].Replace(".aspx", "").ToLower() +
                                  "\"]";
            }

            if (GlobalSettings.HideTopLevelNodeFromPath && requestRawUrl.Length == 2)
            {
                _tempQuery += " | " + pageXPathQueryStart + "/node [" + _urlName + " = \"" +
                              requestRawUrl[1].Replace(".aspx", "").ToLower() + "\"]";
            }
            HttpContext.Current.Trace.Write("umbracoRequestHandler", "xpath: '" + _tempQuery + "'");

            if (checkDomain)
                return _tempQuery;
            else if (!rootAdded)
                return pageXPathQueryStart + _tempQuery;
            else
                return _tempQuery;
        }

        public requestHandler(XmlDocument _umbracoContent, String url)
        {
            HttpContext.Current.Trace.Write("request handler", "current url '" + url + "'");
            bool getByID = false;
            string currentDomain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (!_urlNameInitialized)
                InitializeUrlName();

            // The url exists in cache, and the domain doesn't exists (which makes it ok to do a cache look up on the url alone)
            if (_processedRequests.ContainsKey(url) && !Domain.Exists(currentDomain))
            {
                getByID = true;
                _pageXPathQuery = _processedRequests[url].ToString();
            }
                // The url including the domain exists in cache
            else if (_processedRequests.ContainsKey(currentDomain + url))
            {
                getByID = true;
                _pageXPathQuery = _processedRequests[currentDomain + url].ToString();
            } 
                // The url isn't cached
            else
            {
                if (url == "")
                {
                    url = "";
                    _pageXPathQuery = CreateXPathQuery(url, true);

                    // Never cache roots
                    _doNotCache = true;
                }
                else
                {
                    // If url is an integer, then asume it's the ID of the page
                    if (url[0] == '/')
                        url = url.Substring(1, url.Length - 1);
                    int result;
                    if (int.TryParse(url, out result))
                    {
                        _pageXPathQuery = url;
                        getByID = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(url))
                        {
                            _pageXPathQuery = CreateXPathQuery(url, true);
                        }
                        else
                        {
                            _pageXPathQuery = Document.GetRootDocuments()[0].Id.ToString();
                            getByID = true;
                        }
                    }
                }
            }

            HttpContext.Current.Trace.Write("umbracoRequestHandler",
                                            string.Format("Just before xPath query ({0}, '{1}')", getByID,
                                                          _pageXPathQuery));

            if (getByID)
                currentPage = content.Instance.XmlContent.GetElementById(_pageXPathQuery.Trim());
            else
            {
                HttpContext.Current.Trace.Write("umbracoRequestHandler",
                                                "pageXPathQueryStart: '" + pageXPathQueryStart + "'");
                currentPage = content.Instance.XmlContent.SelectSingleNode(pageXPathQueryStart + _pageXPathQuery);
                if (currentPage == null)
                {
                    // If no node found, then try with a relative page query
                    currentPage = content.Instance.XmlContent.SelectSingleNode("/" + _pageXPathQuery);
                }

                // Add to url cache
                if (currentPage != null && !_doNotCache)
                {
                    string prefixUrl = "";
                    if (Domain.Exists(currentDomain))
                        prefixUrl = currentDomain;
                    if (url.Substring(0, 1) != "/")
                        url = "/" + url;
                    if (_processedRequests.ContainsKey(prefixUrl + url))
                        _processedRequests.Remove(prefixUrl + url);
                    _processedRequests.Add(prefixUrl + url, currentPage.Attributes.GetNamedItem("id").Value);
                    HttpContext.Current.Trace.Write("umbracoRequestHandler",
                                                    "Adding to cache... ('" + prefixUrl + url + "')");
                }
            }

            if (currentPage == null)
            {
                // No node found, try custom url handlers defined in /config/404handlers.config
                if (_customHandlers == null)
                {
                    _customHandlers = new XmlDocument();
                    _customHandlers.Load(
                        HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../config/404handlers.config"));
                }

                for (int i = 0; i < _customHandlers.DocumentElement.ChildNodes.Count; i++)
                {
                    // Load handler
                    string _chAssembly =
                        _customHandlers.DocumentElement.ChildNodes[i].Attributes.GetNamedItem("assembly").Value;
                    string _chType = _customHandlers.DocumentElement.ChildNodes[i].Attributes.GetNamedItem("type").Value;
                    // check for namespace
                    string _chNameSpace = _chAssembly;
                    if (_customHandlers.DocumentElement.ChildNodes[i].Attributes.GetNamedItem("namespace") != null)
                        _chNameSpace =
                            _customHandlers.DocumentElement.ChildNodes[i].Attributes.GetNamedItem("namespace").Value;
                    try
                    {
                        // Reflect to execute and check whether the type is umbraco.main.IFormhandler
                        HttpContext.Current.Trace.Write("notFoundHandler",
                                                        string.Format("Trying NotFoundHandler '{0}.{1}'...", _chAssembly,
                                                                      _chType));
                        Assembly assembly =
                            Assembly.LoadFrom(
                                HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../bin/" + _chAssembly +
                                                                   ".dll"));
                        Type type = assembly.GetType(_chNameSpace + "." + _chType);
                        INotFoundHandler typeInstance = Activator.CreateInstance(type) as INotFoundHandler;
                        if (typeInstance != null)
                        {
                            typeInstance.Execute(url);
                            if (typeInstance.redirectID > 0)
                            {
                                int redirectID = typeInstance.redirectID;
                                currentPage = content.Instance.XmlContent.GetElementById(redirectID.ToString());
                                HttpContext.Current.Trace.Write("notFoundHandler",
                                                                string.Format(
                                                                    "NotFoundHandler '{0}.{1} found node matching {2} with id: {3}",
                                                                    _chAssembly, _chType, url, redirectID));

                                // check for caching
                                if (typeInstance.CacheUrl)
                                {
                                    if (url.Substring(0, 1) != "/")
                                        url = "/" + url;
                                    _processedRequests.Add(url, redirectID.ToString());
                                    HttpContext.Current.Trace.Write("notFoundHandler",
                                                                    string.Format("Added to cache '{0}', '{1}'...", url,
                                                                                  redirectID));
                                }
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        HttpContext.Current.Trace.Warn("notFoundHandler",
                                                       "Error implementing notfoundHandler '" + _chAssembly + "." +
                                                       _chType + "'", e);
                    }
                }
            }

            HttpContext.Current.Trace.Write("umbracoRequestHandler", "After xPath query");

            // Check access
            HttpContext.Current.Trace.Write("umbracoRequestHandler", "Access checking started");
            if (currentPage != null)
            {
                if (
                    Access.IsProtected(int.Parse(currentPage.Attributes.GetNamedItem("id").Value),
                                       currentPage.Attributes.GetNamedItem("path").Value))
                {
                    HttpContext.Current.Trace.Write("umbracoRequestHandler", "Page protected");

                    Member m = Member.GetCurrentMember();
                    if (m == null)
                    {
                        HttpContext.Current.Trace.Write("umbracoRequestHandler",
                                                        "Not logged in - redirecting to login page...");
                        currentPage =
                            content.Instance.XmlContent.GetElementById(
                                Access.GetLoginPage(currentPage.Attributes.GetNamedItem("path").Value).ToString());
                    }
                    else
                    {
                        if (!Access.HasAccess(int.Parse(currentPage.Attributes.GetNamedItem("id").Value), m))
                        {
                            HttpContext.Current.Trace.Write("umbracoRequestHandler",
                                                            "Member has not access - redirecting to error page...");
                            currentPage =
                                content.Instance.XmlContent.GetElementById(
                                    Access.GetErrorPage(currentPage.Attributes.GetNamedItem("path").Value).ToString());
                        }
                    }
                }
                else
                    HttpContext.Current.Trace.Write("umbracoRequestHandler", "Page not protected");
            }
            HttpContext.Current.Trace.Write("umbracoRequestHandler", "Access checking ended");

            // Run formhandler
            if (HttpContext.Current.Request["umbracoAction"] != null)
                if (HttpContext.Current.Request["umbracoAction"] != string.Empty)
                {
                    if (HttpContext.Current.Items["redir"] == null)
                    {
                        string[] actions = helper.Request("umbracoAction").Split(',');
                        foreach (string action in actions)
                        {
                            formHandler fh = new formHandler(action);
                            int redirectID = fh.Execute();
                            if (redirectID > -1)
                            {
                                HttpContext.Current.Items["redir"] = "1";
                                requestHandler redirHandler = new requestHandler(null, redirectID.ToString());
                                if (redirHandler.currentPage != null)
                                    currentPage = redirHandler.currentPage;
                            }
                        }
                    }
                }
        }
    }

    public class SearchForAlias : INotFoundHandler
    {
        private int _redirectID = -1;
        private bool _cacheUrl = true;

        #region INotFoundHandler Members

        public bool Execute(string url)
        {
            bool _succes = false;
            string tempUrl = url;
            if (tempUrl.Length > 0)
            {
                if (tempUrl.Substring(0, 1) == "/")
                    tempUrl = tempUrl.Substring(1, tempUrl.Length - 1);
                HttpContext.Current.Trace.Write("urlAlias", "'" + tempUrl + "'");

                // Check for domain
                string currentDomain = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                string prefixXPath = "";
                if (Domain.Exists(currentDomain))
                {
                    prefixXPath = string.Format("//node [@id = '{0}']", Domain.GetRootFromDomain(currentDomain));
                    _cacheUrl = false;
                }


                XmlNode redir =
                    content.Instance.XmlContent.DocumentElement.SelectSingleNode(
                        prefixXPath + "//node [contains(concat(',',data [@alias = 'umbracoUrlAlias'],','),'," +
                        tempUrl.Replace(".aspx", string.Empty).ToLower() +
                        ",')]");
                if (redir != null)
                {
                    _succes = true;
                    _redirectID = int.Parse(redir.Attributes.GetNamedItem("id").Value);
                }
            }
            return _succes;
        }

        public bool CacheUrl
        {
            get { return _cacheUrl; }
        }

        public int redirectID
        {
            get
            {
                // TODO:  Add SearchForAlias.redirectID getter implementation
                return _redirectID;
            }
        }

        #endregion
    }

    public class SearchForProfile : INotFoundHandler
    {
        private static int _profileId = -1;

        private int _redirectID = -1;

        #region INotFoundHandler Members

        public bool CacheUrl
        {
            get
            {
                // Do not cache profile urls, we need to store the login name
                return false;
            }
        }

        public bool Execute(string url)
        {
            bool _succes = false;
            url = url.Replace(".aspx", string.Empty);
            if (url.Length > 0)
            {
                if (url.Substring(0, 1) == "/")
                    url = url.Substring(1, url.Length - 1);

                if (url.IndexOf("/") > 0)
                {
                    // Check if we're at the profile page
                    if (url.Substring(0, url.IndexOf("/")) == GlobalSettings.ProfileUrl)
                    {
                        if (_profileId < 0)
                        {
                            // /root added to query to solve umbRuntime bug
                            string _tempQuery =
                                requestHandler.CreateXPathQuery(url.Substring(0, url.IndexOf("/")), false);
                            _profileId =
                                int.Parse(
                                    content.Instance.XmlContent.SelectSingleNode(_tempQuery).Attributes.GetNamedItem(
                                        "id").Value);
                        }

                        HttpContext.Current.Items["umbMemberLogin"] =
                            url.Substring(url.IndexOf("/") + 1, url.Length - url.IndexOf("/") - 1);
                        _succes = true;
                        _redirectID = _profileId;
                    }
                }
            }
            return _succes;
        }

        public int redirectID
        {
            get
            {
                // TODO:  Add SearchForProfile.redirectID getter implementation
                return _redirectID;
            }
        }

        #endregion
    }

    public class SearchForTemplate : INotFoundHandler
    {
        private int _redirectID = -1;

        #region INotFoundHandler Members

        public bool CacheUrl
        {
            get
            {
                // Do not cache profile urls, we need to store the login name
                return false;
            }
        }

        public bool Execute(string url)
        {
            bool _succes = false;
            url = url.Replace(".aspx", string.Empty);
            string currentDomain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            if (url.Length > 0)
            {
                if (url.Substring(0, 1) == "/")
                    url = url.Substring(1, url.Length - 1);

                if (url.IndexOf("/") > 0 || Domain.Exists(currentDomain))
                {
                    XmlNode urlNode = null;
                    string templateAlias = "";

                    // We're at domain root
                    if (url.IndexOf("/") == -1) {
                        urlNode = content.Instance.XmlContent.GetElementById(Domain.GetRootFromDomain(currentDomain).ToString());
                        templateAlias = url.ToLower();
                    }
                    else
                    {
                        string theRealUrl = url.Substring(0, url.LastIndexOf("/"));
                        string realUrlXPath = requestHandler.CreateXPathQuery(theRealUrl, true);
                        urlNode = content.Instance.XmlContent.SelectSingleNode(realUrlXPath);
                        templateAlias =
                            url.Substring(url.LastIndexOf("/") + 1, url.Length - url.LastIndexOf(("/")) - 1).ToLower();
                    }


                    if (urlNode != null)
                    {

                        if (Template.GetTemplateIdFromAlias(templateAlias) != 0)
                        {
                            _redirectID = int.Parse(urlNode.Attributes.GetNamedItem("id").Value);
                            HttpContext.Current.Items["altTemplate"] =
                                templateAlias;
                            HttpContext.Current.Trace.Write("umbraco.altTemplateHandler",
                                                            string.Format("Templated changed to: '{0}'",
                                                                          HttpContext.Current.Items["altTemplate"]));
                            _succes = true;
                        }
                    }
                }
            }
            return _succes;
        }

        public int redirectID
        {
            get
            {
                // TODO:  Add SearchForProfile.redirectID getter implementation
                return _redirectID;
            }
        }

        #endregion
    }

    public class handle404 : INotFoundHandler
    {
        #region INotFoundHandler Members

        private int _redirectID = 0;

        public bool CacheUrl
        {
            get
            {
                // TODO:  Add Class1.CacheUrl getter implementation
                return false;
            }
        }

        public bool Execute(string url)
        {
            // TODO:  Add Class1.Execute implementation
            try
            {
                Log.Add(LogTypes.NotFound, User.GetUser(0), -1,
                        url + " (from '" + HttpContext.Current.Request.UrlReferrer + "')");
                _redirectID = int.Parse(UmbracoSettings.Default404Page);
                HttpContext.Current.Response.StatusCode = 404;
                return true;
            }
            catch (Exception err)
            {
                Log.Add(LogTypes.Debug, User.GetUser(0), -1, err.ToString());
                return false;
            }
        }

        public int redirectID
        {
            get
            {
                // TODO:  Add Class1.redirectID getter implementation
                return _redirectID;
            }
        }

        #endregion
    }
}