using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using CookComputing.Blogger;
using CookComputing.MetaWeblog;
using CookComputing.XmlRpc;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.media;
using umbraco.cms.businesslogic.property;
using umbraco.cms.businesslogic.propertytype;
using umbraco.cms.businesslogic.web;
using umbraco.presentation.channels.businesslogic;
using Post=CookComputing.MetaWeblog.Post;

namespace umbraco.presentation.channels
{
    public abstract class UmbracoMetaWeblogAPI : XmlRpcService, IMetaWeblog
    {
        [XmlRpcMethod("blogger.deletePost",
            Description = "Deletes a post.")]
        [return : XmlRpcReturnValue(Description = "Always returns true.")]
        public bool deletePost(
            string appKey,
            string postid,
            string username,
            string password,
            [XmlRpcParameter(
                Description = "Where applicable, this specifies whether the blog "
                              + "should be republished after the post has been deleted.")] bool publish)
        {
            if (User.validateCredentials(username, password, false))
            {
                Channel userChannel = new Channel(username);
                new Document(int.Parse(postid)).delete();
                library.UnPublishSingleNode(int.Parse(postid));
                return true;
            }
            return false;
        }

        public object editPost(
            string postid,
            string username,
            string password,
            Post post,
            bool publish)
        {
            if (User.validateCredentials(username, password, false))
            {
                Channel userChannel = new Channel(username);
                Document doc = new Document(Convert.ToInt32(postid));



                doc.Text = HttpContext.Current.Server.HtmlDecode(post.title);

                // Excerpt
                if (userChannel.FieldExcerptAlias != null && userChannel.FieldExcerptAlias != "")
                    doc.getProperty(userChannel.FieldExcerptAlias).Value = post.mt_excerpt;

                if (UmbracoSettings.TidyEditorContent)
                    doc.getProperty(userChannel.FieldDescriptionAlias).Value = library.Tidy(post.description, false);
                else
                    doc.getProperty(userChannel.FieldDescriptionAlias).Value = post.description;

                updateCategories(doc, post, userChannel);


                if (publish)
                {
                    doc.Publish(new User(username));
                    library.PublishSingleNode(doc.Id);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void updateCategories(Document doc, Post post, Channel userChannel)
        {
            if (userChannel.FieldCategoriesAlias != null && userChannel.FieldCategoriesAlias != "")
            {
                ContentType blogPostType = ContentType.GetByAlias(userChannel.DocumentTypeAlias);
                PropertyType categoryType = blogPostType.getPropertyType(userChannel.FieldCategoriesAlias);

                String[] categories = post.categories;
                string categoryValue = "";
                for (int i = 0; i < categories.Length; i++)
                {
                    PreValue pv = new PreValue(categoryType.DataTypeDefinition.Id, categories[i]);
                    categoryValue += pv.Id + ",";
                }
                if (categoryValue.Length > 0)
                    categoryValue = categoryValue.Substring(0, categoryValue.Length - 1);

                doc.getProperty(userChannel.FieldCategoriesAlias).Value = categoryValue;
            }
        }

        public CategoryInfo[] getCategories(
            string blogid,
            string username,
            string password)
        {
            if (User.validateCredentials(username, password, false))
            {
                Channel userChannel = new Channel(username);
                if (userChannel.FieldCategoriesAlias != null && userChannel.FieldCategoriesAlias != "")
                {
                    // Find the propertytype via the document type
                    ContentType blogPostType = ContentType.GetByAlias(userChannel.DocumentTypeAlias);
                    PropertyType categoryType = blogPostType.getPropertyType(userChannel.FieldCategoriesAlias);

                    SortedList categories = PreValues.GetPreValues(categoryType.DataTypeDefinition.Id);
                    CategoryInfo[] returnedCategories = new CategoryInfo[categories.Count];
                    IDictionaryEnumerator ide = categories.GetEnumerator();
                    int counter = 0;
                    while (ide.MoveNext())
                    {
                        PreValue category = (PreValue) ide.Value;
                        CategoryInfo ci = new CategoryInfo();
                        ci.title = category.Value;
                        ci.categoryid = category.Id.ToString();
                        ci.description = "";
                        ci.rssUrl = "";
                        ci.htmlUrl = "";
                        returnedCategories[counter] = ci;
                        counter++;
                    }

                    return returnedCategories;
                }
            }

            throw new ArgumentException("Categories doesn't work for this channel. Contact your umbraco administrator.");
        }

        public Post getPost(
            string postid,
            string username,
            string password)
        {
            if (User.validateCredentials(username, password, false))
            {
                Channel userChannel = new Channel(username);
                Document d = new Document(int.Parse(postid));
                Post p = new Post();
                p.title = d.Text;
                p.description = d.getProperty(userChannel.FieldDescriptionAlias).Value.ToString();

                // Excerpt
                if (userChannel.FieldExcerptAlias != null && userChannel.FieldExcerptAlias != "")
                    p.mt_excerpt = d.getProperty(userChannel.FieldExcerptAlias).Value.ToString();
                
                // Categories

                p.postid = postid;
                p.permalink = library.NiceUrl(d.Id);
                p.dateCreated = d.CreateDateTime;
                p.link = p.permalink;
                return p;
            }
            else
                throw new ArgumentException(string.Format("Error retriving post with id: '{0}'", postid));
        }

        public Post[] getRecentPosts(
            string blogid,
            string username,
            string password,
            int numberOfPosts)
        {
            if (User.validateCredentials(username, password, false))
            {
                ArrayList blogPosts = new ArrayList();
                ArrayList blogPostsObjects = new ArrayList();

                User u = new User(username);
                Channel userChannel = new Channel(u.Id);


                Document rootDoc;
                if (userChannel.StartNode > 0)
                    rootDoc = new Document(userChannel.StartNode);
                else
                    rootDoc = new Document(u.StartNodeId);


                foreach (Document d in rootDoc.Children)
                {
                    int count = 0;
                    blogPosts.AddRange(
                        findBlogPosts(userChannel, d, u.Name, ref count, numberOfPosts, userChannel.FullTree));
                }

                blogPosts.Sort(new DocumentSortOrderComparer());

                foreach (Object o in blogPosts)
                {
                    Document d = (Document) o;
                    Post p = new Post();
                    p.dateCreated = d.CreateDateTime;
                    p.userid = username;
                    p.title = d.Text;
                    p.permalink = library.NiceUrl(d.Id);
                    p.description = d.getProperty(userChannel.FieldDescriptionAlias).Value.ToString();
                    p.link = library.NiceUrl(d.Id);
                    p.postid = d.Id.ToString();

                    if (userChannel.FieldCategoriesAlias != null && userChannel.FieldCategoriesAlias != "" &&
                        d.getProperty(userChannel.FieldCategoriesAlias) != null &&
                        d.getProperty(userChannel.FieldCategoriesAlias).Value != null && 
                        d.getProperty(userChannel.FieldCategoriesAlias).Value.ToString() != "")
                    {
                        String categories = d.getProperty(userChannel.FieldCategoriesAlias).Value.ToString();
                        char[] splitter = {','};
                        String[] categoryIds = categories.Split(splitter);
                        p.categories = categoryIds;
                    }

                    blogPostsObjects.Add(p);
                }


                return (Post[]) blogPostsObjects.ToArray(typeof (Post));
            }
            else
            {
                return null;
            }
        }

        protected ArrayList findBlogPosts(Channel userChannel, Document d, String userName, ref int count, int max,
                                          bool fullTree)
        {
            ArrayList list = new ArrayList();

            ContentType ct = d.ContentType;

            if (ct.Alias.Equals(userChannel.DocumentTypeAlias) &
                (count < max))
            {
                list.Add(d);
                count = count + 1;
            }

            if (d.Children != null && d.Children.Length > 0 && fullTree)
                foreach (Document child in d.Children)
                {
                    if (count < max)
                    {
                        list.AddRange(findBlogPosts(userChannel, child, userName, ref count, max, true));
                    }
                }

            return list;
        }

        public string newPost(
            string blogid,
            string username,
            string password,
            Post post,
            bool publish)
        {
            if (User.validateCredentials(username, password, false))
            {
                Channel userChannel = new Channel(username);
                User u = new User(username);
                Document doc =
                    Document.MakeNew(HttpContext.Current.Server.HtmlDecode(post.title),
                                     DocumentType.GetByAlias(userChannel.DocumentTypeAlias), u,
                                     userChannel.StartNode);


                // Excerpt
                if (userChannel.FieldExcerptAlias != null && userChannel.FieldExcerptAlias != "")
                    doc.getProperty(userChannel.FieldExcerptAlias).Value = post.mt_excerpt;


                // Description
                if (UmbracoSettings.TidyEditorContent)
                    doc.getProperty(userChannel.FieldDescriptionAlias).Value = library.Tidy(post.description, false);
                else
                    doc.getProperty(userChannel.FieldDescriptionAlias).Value = post.description;

                // Categories
                updateCategories(doc, post, userChannel);

                if (publish)
                {
                    doc.Publish(new User(username));
                    library.PublishSingleNode(doc.Id);
                }
                return doc.Id.ToString();
            }
            else
                throw new ArgumentException("Error creating post");
        }


        protected UrlData newMediaObjectLogic(
            string blogid,
            string username,
            string password,
            FileData file)
        {
            if (User.validateCredentials(username, password, false))
            {
                User u = new User(username);
                Channel userChannel = new Channel(username);
                UrlData fileUrl = new UrlData();
                if (userChannel.ImageSupport)
                {
                    Media rootNode;
                    if (userChannel.MediaFolder > 0)
                        rootNode = new Media(userChannel.MediaFolder);
                    else
                        rootNode = new Media(u.StartMediaId);

                    // Create new media
                    Media m = Media.MakeNew(file.name, MediaType.GetByAlias(userChannel.MediaTypeAlias), u, rootNode.Id);

                    Property fileObject = m.getProperty(userChannel.MediaTypeFileProperty);
                    string _fullFilePath;
                    string filename = file.name.Replace("/", "_");
                    // Generate file
                    if (UmbracoSettings.UploadAllowDirectories)
                    {
                        // Create a new folder in the /media folder with the name /media/propertyid
                        Directory.CreateDirectory(
                            HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../media/" + fileObject.Id));
                        _fullFilePath =
                            HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../media/" + fileObject.Id +
                                                               "/" + filename);
                        fileObject.Value = "/media/" + fileObject.Id + "/" + filename;
                    }
                    else
                    {
                        filename = fileObject.Id + "-" + filename;
                        _fullFilePath =
                            HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../media/" + filename);
                        fileObject.Value = "/media/" + filename;
                    }
                    fileUrl.url = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] +
                                  fileObject.Value.ToString();

                    File.WriteAllBytes(_fullFilePath, file.bits);

                    // Try updating standard file values
                    try
                    {
                        string orgExt = "";
                        // Size
                        if (m.getProperty("umbracoBytes") != null)
                            m.getProperty("umbracoBytes").Value = file.bits.Length;
                        // Extension
                        if (m.getProperty("umbracoExtension") != null)
                        {
                            orgExt =
                                ((string)
                                 file.name.Substring(file.name.LastIndexOf(".") + 1,
                                                     file.name.Length - file.name.LastIndexOf(".") - 1));
                            m.getProperty("umbracoExtension").Value = orgExt.ToLower();
                        }
                        // Width and Height
                        // Check if image and then get sizes, make thumb and update database
                        if (m.getProperty("umbracoWidth") != null && m.getProperty("umbracoHeight") != null &&
                            ",jpeg,jpg,gif,bmp,png,tiff,tif,".IndexOf("," + orgExt.ToLower() + ",") > 0)
                        {
                            int fileWidth;
                            int fileHeight;

                            FileStream fs = new FileStream(_fullFilePath,
                                                           FileMode.Open, FileAccess.Read, FileShare.Read);

                            Image image = Image.FromStream(fs);
                            fileWidth = image.Width;
                            fileHeight = image.Height;
                            fs.Close();
                            try
                            {
                                m.getProperty("umbracoWidth").Value = fileWidth.ToString();
                                m.getProperty("umbracoHeight").Value = fileHeight.ToString();
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }

                    return fileUrl;
                }
                else
                    throw new ArgumentException(
                        "Image Support is turned off in this channel. Modify channel settings in umbraco to enable image support.");
            }
            return new UrlData();
        }

        [XmlRpcMethod("blogger.getUsersBlogs",
            Description = "Returns information on all the blogs a given user "
                          + "is a member.")]
        public BlogInfo[] getUsersBlogs(
            string appKey,
            string username,
            string password)
        {
            if (User.validateCredentials(username, password, false))
            {
                BlogInfo[] blogs = new BlogInfo[1];
                User u = new User(username);
                Channel userChannel = new Channel(u.Id);
                Document rootDoc;
                if (userChannel.StartNode > 0)
                    rootDoc = new Document(userChannel.StartNode);
                else
                    rootDoc = new Document(u.StartNodeId);

                BlogInfo bInfo = new BlogInfo();
                bInfo.blogName = userChannel.Name;
                bInfo.blogid = rootDoc.Id.ToString();
                bInfo.url = HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + library.NiceUrl(rootDoc.Id);
                blogs[0] = bInfo;

                return blogs;
            }

            throw new ArgumentException(string.Format("No data found for user with username: '{0}'", username));
        }
    }
}