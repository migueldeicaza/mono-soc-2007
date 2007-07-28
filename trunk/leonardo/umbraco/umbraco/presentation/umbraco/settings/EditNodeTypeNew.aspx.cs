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

namespace umbraco.settings
{
	public partial class EditContentTypeNew : BasePages.UmbracoEnsuredPage
	{
		protected controls.ContentTypeControlNew ContentTypeControlNew1;
		cms.businesslogic.web.DocumentType dt;
	

		private DataTable dtTemplates = new DataTable();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			dt = new cms.businesslogic.web.DocumentType(int.Parse(Request.QueryString["id"]));
			if (!Page.IsPostBack)
				bindTemplates();

		}

		private void bindTemplates() {
			cms.businesslogic.template.Template[] selectedTemplates = dt.allowedTemplates;

			DataTable dtAllowedTemplates = new DataTable();
			dtTemplates.Columns.Add("name");
			dtTemplates.Columns.Add("id");
			dtTemplates.Columns.Add("selected");

		
			ddlTemplates.Items.Add(new ListItem("Ingen template","0"));
			foreach (cms.businesslogic.template.Template t in cms.businesslogic.template.Template.getAll()) 
			{
				DataRow dr = dtTemplates.NewRow();
				dr["name"] = t.Text;
				dr["id"] = t.Id;
				dr["selected"] = false;
				foreach (cms.businesslogic.template.Template t1 in selectedTemplates) 
					if (t1 != null && t1.Id==t.Id)
						dr["selected"] = true;
					
				dtTemplates.Rows.Add(dr);
				
			}			
	
			templateList.Items.Clear();

			foreach (DataRow dr in dtTemplates.Rows)
			{
				ListItem li = new ListItem(dr["name"].ToString(),dr["id"].ToString());
				if (bool.Parse(dr["selected"].ToString()))
					li.Selected = true;
				templateList.Items.Add(li);
			}
			
			ddlTemplates.Items.Clear();
			foreach (DataRow dr in dtTemplates.Rows)
			{
				ListItem li = new ListItem(dr["name"].ToString(),dr["id"].ToString());
				if (li.Value == dt.DefaultTemplate.ToString())
					li.Selected = true;
				if (bool.Parse(dr["selected"].ToString()))
					ddlTemplates.Items.Add(li);
			}
			if (ddlTemplates.Items.Count > 0) ddlTemplates.Enabled = true;
			else ddlTemplates.Enabled = false;

			// Add choose to ddlTemplates
			ddlTemplates.Items.Insert(0, new ListItem(ui.Text("choose") + "...", "0"));

		}


		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			bool handled = false;
			if (args is controls.SaveClickEventArgs) 
			{
				controls.SaveClickEventArgs e = (controls.SaveClickEventArgs) args;
				if (e.Message == "Saved") 
				{
					base.speechBubble(BasePages.BasePage.speechBubbleIcon.save,"Documenttype saved","");

					ArrayList tmp = new ArrayList();

					foreach (ListItem li in templateList.Items) {
						if (li.Selected) tmp.Add(new cms.businesslogic.template.Template(int.Parse(li.Value)));
					}

					cms.businesslogic.template.Template[] tt = new cms.businesslogic.template.Template[tmp.Count];
						for(int i = 0; i < tt.Length;i++) {
							tt[i] = (cms.businesslogic.template.Template) tmp[i];
						}
					
					dt.allowedTemplates = tt;


					if (dt.allowedTemplates.Length > 0 && ddlTemplates.SelectedIndex >= 0) 
					{
						dt.DefaultTemplate = int.Parse(ddlTemplates.SelectedValue);
					}
					else
						dt.RemoveDefaultTemplate();

					bindTemplates();
				} 
				else 
				{
					base.speechBubble(BasePages.BasePage.speechBubbleIcon.save,e.Message,"");
				}
				handled = true;
			}
			return handled;
		}

		protected void dgTemplate_itemdatabound(object sender,DataGridItemEventArgs e) 
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
			{
				((CheckBox)e.Item.FindControl("ckbAllowTemplate")).Checked = true;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			ContentTypeControlNew1.InfoTabPage.Controls.Add(tmpPane);
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
