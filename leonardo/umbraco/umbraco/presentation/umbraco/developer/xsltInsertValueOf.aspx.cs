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
using umbraco.cms.businesslogic.propertytype;

namespace umbraco.developer
{
	/// <summary>
	/// Summary description for xsltInsertValueOf.
	/// </summary>
	public partial class xsltInsertValueOf : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
		    ArrayList preValuesSource = new ArrayList();

            // Attributes
            string[] attributes = {"@id", "@parentID", "@level", "@writerID", "@nodeType", "@template", "@sortOrder", "@createDate", "@updateDate", "@nodeName", "@urlName", "@writerName", "@nodeTypeAlias", "@path"};
            foreach (string att in attributes)
                preValuesSource.Add(att);

            // generic properties
            string existingGenProps = ",";
            foreach (PropertyType pt in PropertyType.GetAll())
                if (!existingGenProps.Contains("," + pt.Alias + ","))
                {
                    preValuesSource.Add(string.Format("data [@alias = '{0}']", pt.Alias));
                    existingGenProps += pt.Alias + ",";
                }


		    preValues.DataSource = preValuesSource;
			preValues.DataBind();
			preValues.Items.Insert(0, new ListItem("Prevalues...", ""));
			preValues.Attributes.Add("onChange", "if (this.value != '') document.getElementById('valueOf').value = this.value");
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
