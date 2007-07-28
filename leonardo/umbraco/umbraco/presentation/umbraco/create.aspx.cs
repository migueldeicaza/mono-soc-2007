using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Xml.XPath;
using System.Xml;

namespace umbraco.cms.presentation
{
	/// <summary>
	/// Summary description for create.
	/// </summary>
	public partial class Create : BasePages.UmbracoEnsuredPage
	{
		protected umbWindow createWindow;
		protected System.Web.UI.WebControls.Label helpText;
		protected System.Web.UI.WebControls.TextBox rename;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.ListBox nodeType;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Load create definitions
			try 
			{
				XmlDocument createDef = new XmlDocument();
				XmlTextReader defReader = new XmlTextReader(Server.MapPath(GlobalSettings.Path+"/config/create/UI.xml"));
				createDef.Load(defReader);
				defReader.Close();

				// Find definition for current nodeType
				XmlNode def = createDef.SelectSingleNode("//nodeType [@alias = '" + Request.QueryString["nodeType"] + "']");
				title.Text = ui.Text("create") + " " + ui.Text(def.SelectSingleNode("./header").FirstChild.Value.ToLower(), base.getUser());
				headerTitle.Text = title.Text;
				UI.Controls.Add(new UserControl().LoadControl(GlobalSettings.Path+ def.SelectSingleNode("./usercontrol").FirstChild.Value));
			} 
			catch {
				throw new ArgumentException("ERROR CREATING CONTROL FOR NODETYPE: " + Request.QueryString["nodeType"]);
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
