namespace dashboardUtilities
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for LatestEdits.
	/// </summary>
	public partial class LatestEdits : System.Web.UI.UserControl
	{

		// Find current user
		public umbraco.BasePages.UmbracoEnsuredPage bp = new umbraco.BasePages.UmbracoEnsuredPage();
		private System.Collections.ArrayList printedIds = new System.Collections.ArrayList();
		private int count = 0;
		private int maxRecords = 5;

		protected void Page_Load(object sender, System.EventArgs e)
		{

			// Put user code to initialize the page here
			Repeater1.DataSource = umbraco.BusinessLogic.Log.GetLog(bp.getUser(), umbraco.BusinessLogic.LogTypes.Save, DateTime.Now.Subtract(new System.TimeSpan(7,0,0,0,0)));
			Repeater1.DataBind();
		}

		public string PrintNodeName(object NodeId, object Date) 
		{
			if (!printedIds.Contains(NodeId) && count < maxRecords) 
			{
				printedIds.Add(NodeId);
				try 
				{
					umbraco.cms.businesslogic.web.Document d = new umbraco.cms.businesslogic.web.Document(int.Parse(NodeId.ToString()));
					string parent = "";
					try 
					{
						if (d.Parent != null)
							parent = " (" + d.Parent.Text + ")";
					} 
					catch {}
					count++;
					return 
						"<a href=\"editContent.aspx?id=" + NodeId.ToString() + "\" style=\"text-decoration: none\"><img src=\"" + umbraco.GlobalSettings.Path + "/images/forward.png\" align=\"absmiddle\" border=\"0\"/> " + d.Text + parent + "</a>. " + umbraco.ui.Text("general", "edited", bp.getUser()) + " " + umbraco.library.ShortDateWithTimeAndGlobal(DateTime.Parse(Date.ToString()).ToString(), umbraco.ui.Culture(bp.getUser())) + "<br/>";
				}
				catch {
					return "";
				}
				
			} else
				return "";
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

		}
		#endregion
	}
}
