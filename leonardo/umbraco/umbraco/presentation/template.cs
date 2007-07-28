using System;
using System.Xml;
using System.Web.Caching;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections;

namespace umbraco
{
	/// <summary>
	/// Holds methods for parsing and building umbraco templates
	/// </summary>
	/// 
	public class template
	{
		#region private variables
		StringBuilder templateOutput = new StringBuilder();

		// Cache
		static System.Web.Caching.Cache templateCache = System.Web.HttpRuntime.Cache;

		private string _templateDesign = "";
		int _masterTemplate = -1;
		private string _templateName = "";

		#endregion

		#region public properties
		public String TemplateContent 

		{
			set 
			{
				templateOutput.Append(value);
			}
			get 
			{
				return templateOutput.ToString();
			}
		}
			
		public int MasterTemplate
		{
			get {return _masterTemplate;}
		}
		#endregion

		#region public methods

		public override string ToString()
		{
			return this._templateName;
		}

		public Control ParseWithControls(page umbPage) 
		{
			System.Web.HttpContext.Current.Trace.Write("umbracoTemplate", "Start parsing");

			if (System.Web.HttpContext.Current.Items["macrosAdded"] == null)
				System.Web.HttpContext.Current.Items.Add("macrosAdded", 0);
			
			StringBuilder tempOutput = templateOutput;

			Control pageLayout = new Control();
			Control pageHeader = new Control();
			Control pageFooter = new Control();
			Control pageContent = new Control();
			System.Web.UI.HtmlControls.HtmlForm pageForm = new System.Web.UI.HtmlControls.HtmlForm();
            System.Web.UI.HtmlControls.HtmlHead pageAspNetHead = new System.Web.UI.HtmlControls.HtmlHead();

			// Find header and footer of page if there is an aspnet-form on page
            if (templateOutput.ToString().ToLower().IndexOf("<?aspnet_form>") > 0 ||
                templateOutput.ToString().ToLower().IndexOf("<?aspnet_form disablescriptmanager=\"true\">") > 0) 
			{
				pageForm.Attributes.Add("method", "post");
				pageForm.Attributes.Add("action", Convert.ToString(System.Web.HttpContext.Current.Items["VirtualUrl"]));

				// Find header and footer from tempOutput
				int aspnetFormTagBegin = tempOutput.ToString().ToLower().IndexOf("<?aspnet_form>");
			    int aspnetFormTagLength = 14;
                int aspnetFormTagEnd = tempOutput.ToString().ToLower().IndexOf("</?aspnet_form>") + 15;

                // check if we should disable the script manager
                if (aspnetFormTagBegin == -1) {
                    aspnetFormTagBegin =
                        templateOutput.ToString().ToLower().IndexOf("<?aspnet_form disablescriptmanager=\"true\">");
                    aspnetFormTagLength = 42;
                }
                else
                {
                    ScriptManager sm = new ScriptManager();
                    sm.ID = "umbracoScriptManager";
                    pageForm.Controls.Add(sm);
                }


                StringBuilder header = new StringBuilder(tempOutput.ToString().Substring(0, aspnetFormTagBegin));

                // Check if there's an asp.net head element in the header
                if (header.ToString().ToLower().Contains("<?aspnet_head>"))
                {
                    StringBuilder beforeHeader = new StringBuilder(header.ToString().Substring(0, header.ToString().ToLower().IndexOf("<?aspnet_head>")));
                    header.Remove(0, header.ToString().ToLower().IndexOf("<?aspnet_head>") + 14);
                    StringBuilder afterHeader = new StringBuilder(header.ToString().Substring(header.ToString().ToLower().IndexOf("</?aspnet_head>") + 15, header.Length - header.ToString().ToLower().IndexOf("</?aspnet_head>") - 15));
                    header.Remove(header.ToString().ToLower().IndexOf("</?aspnet_head>"), header.Length - header.ToString().ToLower().IndexOf("</?aspnet_head>"));

                    // Find the title from head
                    MatchCollection matches = Regex.Matches(header.ToString(), @"<title>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    if (matches.Count > 0)
                    {
                        StringBuilder titleText = new StringBuilder();
                        HtmlTextWriter titleTextTw = new HtmlTextWriter(new System.IO.StringWriter(titleText));
                        parseStringBuilder(new StringBuilder(matches[0].Groups[1].Value), umbPage).RenderControl(titleTextTw);
                        pageAspNetHead.Title = titleText.ToString();
                        header = new StringBuilder(header.ToString().Replace(matches[0].Value, ""));
                    }
                    
                    pageAspNetHead.Controls.Add(parseStringBuilder(header, umbPage));
                    pageAspNetHead.ID = "head1";

                    // build the whole header part
                    pageHeader.Controls.Add(parseStringBuilder(beforeHeader, umbPage));
                    pageHeader.Controls.Add(pageAspNetHead);
                    pageHeader.Controls.Add(parseStringBuilder(afterHeader, umbPage));

                } else
				    pageHeader.Controls.Add(parseStringBuilder(header, umbPage));


				pageFooter.Controls.Add(parseStringBuilder(new StringBuilder(tempOutput.ToString().Substring(aspnetFormTagEnd, tempOutput.Length-aspnetFormTagEnd)), umbPage));
                tempOutput.Remove(0, aspnetFormTagBegin + aspnetFormTagLength);
				aspnetFormTagEnd = tempOutput.ToString().ToLower().IndexOf("</?aspnet_form>");
				tempOutput.Remove(aspnetFormTagEnd, tempOutput.Length-aspnetFormTagEnd);
				

				//throw new ArgumentException(tempOutput.ToString());
				pageForm.Controls.Add(parseStringBuilder(tempOutput, umbPage));

				pageContent.Controls.Add(pageHeader);
				pageContent.Controls.Add(pageForm);
				pageContent.Controls.Add(pageFooter);
				return pageContent;

			}  
			else
				return parseStringBuilder(tempOutput, umbPage);

		}

		public Control parseStringBuilder(StringBuilder tempOutput, page umbPage) {

			Control pageContent = new Control();

			bool stop = false;
			bool debugMode = false;
			if (GlobalSettings.DebugMode && (helper.Request("umbdebugshowtrace") != "" || helper.Request("umbdebug") != ""))
				debugMode = true;
			while (!stop)
			{
				System.Web.HttpContext.Current.Trace.Write("template", "Begining of parsing rutine...");
				int tagIndex = tempOutput.ToString().ToLower().IndexOf("<?umbraco");
				if (tagIndex > -1) 
				{
					String tempElementContent = "";
					pageContent.Controls.Add(new LiteralControl(tempOutput.ToString().Substring(0,tagIndex)));

					tempOutput.Remove(0, tagIndex);
					
					String tag = tempOutput.ToString().Substring(0, tempOutput.ToString().IndexOf(">")+1);
					Hashtable attributes = helper.ReturnAttributes(tag);

					// Check whether it's a single tag (<?.../>) or a tag with children (<?..>...</?...>)
					if (tag.Substring(tag.Length-2, 1) != "/" && tag.IndexOf(" ") > -1)
					{
						String closingTag = "</" + (tag.Substring(1, tag.IndexOf(" ")-1)) + ">";
						// Tag with children are only used when a macro is inserted by the umbraco-editor, in the
						// following format: "<?UMBRACO_MACRO ...><IMG SRC="..."..></?UMBRACO_MACRO>", so we
						// need to delete extra information inserted which is the image-tag and the closing
						// umbraco_macro tag
						if (tempOutput.ToString().IndexOf(closingTag) > -1) 
						{
							tempOutput.Remove(0, tempOutput.ToString().IndexOf(closingTag));
						}
					}

				

					System.Web.HttpContext.Current.Trace.Write("umbTemplate", "Outputting item: " + tag);

					// Handle umbraco macro tags
					if (tag.ToString().ToLower().IndexOf("umbraco_macro") > -1) 
					{
						if (debugMode)
							pageContent.Controls.Add(new LiteralControl("<div title=\"Macro Tag: '" + System.Web.HttpContext.Current.Server.HtmlEncode(tag) + "'\" style=\"border: 1px solid #009;\">"));

						macro tempMacro;
						String macroID = FindAttribute(attributes, "macroID");
						if (macroID != "") 
							tempMacro = getMacro(macroID);
						else
							tempMacro = macro.ReturnFromAlias(FindAttribute(attributes, "macroAlias"));

						if (tempMacro != null) {

							try 
							{
								Control c = tempMacro.renderMacro(attributes, umbPage);
								if (c != null)
									pageContent.Controls.Add(c);
								else
									System.Web.HttpContext.Current.Trace.Warn("Template", "Result of macro " + tempMacro.Name + " is null");

							} 
							catch (Exception e)
							{
								System.Web.HttpContext.Current.Trace.Warn("Template", "Error adding macro " + tempMacro.Name, e);
							}
						}
						if (debugMode)
							pageContent.Controls.Add(new LiteralControl("</div>"));
					}
					else 
					{
						if (tag.ToLower().IndexOf("umbraco_getitem") > -1) 
						{
							try 
							{
								if (FindAttribute(attributes, "id") != "" && int.Parse(helper.FindAttribute(attributes, "id")) != 0) 
								{
									cms.businesslogic.Content c = new umbraco.cms.businesslogic.Content(int.Parse(helper.FindAttribute(attributes, "id")));
									item umbItem = new item(c.getProperty(FindAttribute(attributes, "field")).Value.ToString(), attributes);
									tempElementContent = umbItem.FieldContent;

									// Check if the content is published
									try 
									{
										cms.businesslogic.web.Document d = (cms.businesslogic.web.Document) c;
										if (!d.Published)
											tempElementContent = "";
									} 
									catch {}
									
								} 
								else 
								{
									// NH adds live editing test stuff
									item umbItem = new item(umbPage, attributes);
									//								item umbItem = new item(umbPage.Elements[helper.FindAttribute(attributes, "field")].ToString(), attributes);
									tempElementContent = umbItem.FieldContent;
								}

								if (debugMode)
									tempElementContent =
										"<div title=\"Field Tag: '" + System.Web.HttpContext.Current.Server.HtmlEncode(tag) + "'\" style=\"border: 1px solid #fc6;\">" + tempElementContent + "</div>";
							} 
							catch (Exception e) 
							{
								System.Web.HttpContext.Current.Trace.Warn("umbracoTemplate", "Error reading element (" + FindAttribute(attributes, "field") + ")", e);
							}
						} 
					}
					tempOutput.Remove(0, tempOutput.ToString().IndexOf(">")+1);
					tempOutput.Insert(0, tempElementContent);
				} 
				else 
				{
						pageContent.Controls.Add(new LiteralControl(tempOutput.ToString()));
					break;
				}

			}

			return pageContent;

		}

        public static string ParseInternalLinks(string pageContents)
        {
            // Parse internal links
            MatchCollection tags = Regex.Matches(pageContents, "href=\"{localLink:?([^\\}' >]+)|href=\"/{localLink:?([^\\}' >]+)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            foreach (Match tag in tags)
                if (tag.Groups.Count > 0)
                {
                    if (tag.Groups[1].Value != "")
                    {
                        string id = tag.Groups[1].Value; //.Remove(tag.Groups[1].Value.Length - 1, 1);
                        string newLink = library.NiceUrl(int.Parse(id));
                        pageContents = pageContents.Replace("{localLink:" + id + "}", newLink);
                    }
                    else
                    {
                        string id = tag.Groups[2].Value; //.Remove(tag.Groups[2].Value.Length - 1, 1);
                        string newLink = library.NiceUrl(int.Parse(id));
                        pageContents = pageContents.Replace("/{localLink:" + id + "}", newLink);
                    }
                }
            return pageContents;
        }

/// <summary>
/// Parses the content of the templateOutput stringbuilder, and matches any tags given in the
/// XML-file /umbraco/config/umbracoTemplateTags.xml. 
/// Replaces the found tags in the StringBuilder object, with "real content"
/// </summary>
/// <param name="umbPage"></param>
		public void Parse(page umbPage) 
		{
			System.Web.HttpContext.Current.Trace.Write("umbracoTemplate", "Start parsing");

			// First parse for known umbraco tags
			// <?UMBRACO_MACRO/> - macros
			// <?UMBRACO_GETITEM/> - print item from page, level, or recursive
			MatchCollection tags = Regex.Matches(templateOutput.ToString(), "<\\?UMBRACO_MACRO[^>]*/>|<\\?UMBRACO_GETITEM[^>]*/>|<\\?(?<tagName>[\\S]*)[^>]*/>", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

			foreach (Match tag in tags) 
			{
				Hashtable attributes = helper.ReturnAttributes(tag.Value.ToString());
				

				if (tag.ToString().ToLower().IndexOf("umbraco_macro") > -1) 
				{
					String macroID = FindAttribute(attributes, "macroID");
					if (macroID != "") 
					{
						macro tempMacro = getMacro(macroID);
						templateOutput.Replace(tag.Value.ToString(), tempMacro.MacroContent.ToString());
					}
				}
				else 
				{
					if (tag.ToString().ToLower().IndexOf("umbraco_getitem") > -1) 
					{
						try 
						{
							String tempElementContent = umbPage.Elements[FindAttribute(attributes, "field")].ToString();
							MatchCollection tempMacros = Regex.Matches(tempElementContent, "<\\?UMBRACO_MACRO(?<attributes>[^>]*)><img[^>]*><\\/\\?UMBRACO_MACRO>", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
							foreach (Match tempMacro in tempMacros) {
								Hashtable tempAttributes = helper.ReturnAttributes(tempMacro.Groups["attributes"].Value.ToString());
								String macroID = FindAttribute(tempAttributes, "macroID");
								if (Convert.ToInt32(macroID) > 0) {
									macro tempContentMacro = getMacro(macroID);
									templateOutput.Replace(tag.Value.ToString(), tempContentMacro.MacroContent.ToString());
								}
								
							}

							templateOutput.Replace(tag.Value.ToString(), tempElementContent);
						} 
						catch (Exception e) 
						{
							System.Web.HttpContext.Current.Trace.Warn("umbracoTemplate", "Error reading element (" + FindAttribute(attributes, "field") + ")", e);
						}
					}
				}
			}

			System.Web.HttpContext.Current.Trace.Write("umbracoTemplate", "Done parsing");		
		}



		#endregion

		#region private methods

		private macro getMacro(String macroID) {
			System.Web.HttpContext.Current.Trace.Write("umbracoTemplate", "Starting macro (" + macroID.ToString() + ")");
			return new macro(Convert.ToInt16(macroID));
		}

		private String FindAttribute(Hashtable attributes, String key) 
		{
			if (attributes[key] != null)
					return attributes[key].ToString();
			else
				return "";
		}

		
		#endregion

		#region constructors

		public template(int templateID)
		{
			if (templateCache["template" + templateID.ToString()] != null)  
			{
				template t = (template) templateCache["template" + templateID];
				this._masterTemplate = t._masterTemplate;
				this._templateDesign = t._templateDesign;
				this._masterTemplate = t._masterTemplate;
				this._templateName = t._templateName;
			} else
			{
				using(SqlDataReader templateData = SqlHelper.ExecuteReader(GlobalSettings.DbDSN, 
					CommandType.Text, "select nodeId, master, text, design from cmsTemplate inner join umbracoNode node on node.id = cmsTemplate.nodeId where nodeId = @templateID", new SqlParameter("@templateID", templateID)))
				{
					if(templateData.Read())
					{
						// Get template master and replace content where the template
						if(!templateData.IsDBNull(templateData.GetOrdinal("master")))
							_masterTemplate = templateData.GetInt32(templateData.GetOrdinal("master"));
						if(!templateData.IsDBNull(templateData.GetOrdinal("text")))
							_templateName = templateData.GetString(templateData.GetOrdinal("text"));
						if(!templateData.IsDBNull(templateData.GetOrdinal("design")))
							_templateDesign = templateData.GetString(templateData.GetOrdinal("design"));
						templateData.Close();
						templateCache.Insert("template" + templateID.ToString(), this);
					}
				}
			}
			checkForMaster(templateID);
		}

		private void checkForMaster(int templateID) 
		{
			// Get template design
			if (_masterTemplate != 0 && _masterTemplate != templateID) 
			{
				template masterTemplateDesign = new template(_masterTemplate);
				if (masterTemplateDesign.TemplateContent.IndexOf("<?UMBRACO_TEMPLATE_LOAD_CHILD/>") > -1
					|| masterTemplateDesign.TemplateContent.IndexOf("<?UMBRACO_TEMPLATE_LOAD_CHILD />") > -1) 
				{
					templateOutput.Append(
						masterTemplateDesign.TemplateContent.Replace("<?UMBRACO_TEMPLATE_LOAD_CHILD/>", 
						_templateDesign).Replace("<?UMBRACO_TEMPLATE_LOAD_CHILD />", _templateDesign)
						);
				} 
				else
					templateOutput.Append(_templateDesign);
			} 
			else 
			{
				if (_masterTemplate == templateID)
					System.Web.HttpContext.Current.Trace.Warn("template", "Master template is the same as the current template. It would course an endless loop!");
				templateOutput.Append(_templateDesign);
			}
		}

		public static void ClearCachedTemplate(int templateID) 
		{
			if (templateCache["template" + templateID.ToString()] != null)
				templateCache.Remove("template" + templateID.ToString());
		}

		public template(String templateContent)
		{
			templateOutput.Append(templateContent);
			_masterTemplate = 0;
		}

		#endregion
	}

	public class templateCacheRefresh : interfaces.ICacheRefresher
	{
		#region ICacheRefresher Members

		public string Name
		{
			get
			{
				// TODO:  Add templateCacheRefresh.Name getter implementation
				return "Template cache refresher";
			}
		}

		public Guid UniqueIdentifier
		{
			get
			{
				// TODO:  Add templateCacheRefresh.UniqueIdentifier getter implementation
				return new Guid ("DD12B6A0-14B9-46e8-8800-C154F74047C8");
			}
		}

		public void RefreshAll()
		{
			// Doesn't do anything
		}

		public void Refresh(Guid Id)
		{
			// Doesn't do anything
		}

		void umbraco.interfaces.ICacheRefresher.Refresh(int Id)
		{
			template.ClearCachedTemplate(Id);
		}

		#endregion

	}

}
