using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace umbraco.presentation.umbraco.create
{
    public partial class script : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sbmt.Text = ui.Text("create");
        }

        private void sbmt_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                string path = helper.Request("nodeID");

                if (path == "init")
                    path = System.Web.HttpContext.Current.Server.MapPath(UmbracoSettings.ScriptFolderPath);

                int createFolder = 0;
                if (scriptType.SelectedValue == "")
                    createFolder = 1;

                string returnPath = presentation.create.dialogHandler_temp.Create(
                    helper.Request("nodeType"),
                    createFolder,
                    path + "�" + rename.Text + "�" + scriptType.SelectedValue);
                
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "refresh", "<script>\nwindow.opener.refreshTree(false, true);\nwindow.close();</script>");
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sbmt.Click += new System.EventHandler(this.sbmt_Click);
            this.Load += new System.EventHandler(this.Page_Load);

            string[] fileTypes = UmbracoSettings.ScriptFileTypes.Split(',');

            foreach (string str in fileTypes)
            {
                scriptType.Items.Add(new ListItem("." + str + " file", str));
            }
        }
        #endregion
    
    }
}