namespace umbraco.controls
{
	using System.IO;
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using System.Web.UI;

	public partial class ContentTypeControlNew : System.Web.UI.UserControl
	{
		public uicontrols.TabPage InfoTabPage;
		
		// General Private members
		private cms.businesslogic.ContentType cType;
		private static string UmbracoPath = GlobalSettings.Path;
			
		// "Tab" tab
		protected uicontrols.Pane Pane8;

		
		// "Structure" tab
		protected controls.DualSelectbox dualAllowedContentTypes = new DualSelectbox();
		
		// "Info" tab
		
		// "Generic properties" tab
		public GenericProperties.GenericPropertyWrapper gp;
		private ArrayList _genericProperties = new ArrayList();
		private ArrayList _sortLists = new ArrayList();
		protected System.Web.UI.WebControls.DataGrid dgGeneralTabProperties;
	
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dragdrop", "<script src=\"" + GlobalSettings.Path + "/js/prototype.js\" type=\"text/javascript\"></script><script src=\"" + GlobalSettings.Path + "/js/dragdrop.js\" type=\"text/javascript\"></script><script src=\"" + GlobalSettings.Path + "/js/effects.js\" type=\"text/javascript\"></script>");
		}

		protected void save_click(object sender, System.Web.UI.ImageClickEventArgs e) 
		{
			cType.Text = txtName.Text;
			cType.Alias = txtAlias.Text;
			cType.IconUrl = ddlIcons.SelectedValue;
		    cType.Description = description.Text;
		    cType.Thumbnail = ddlThumbnails.SelectedValue;

			saveProperties();
			SaveTabs();
			SaveAllowedChildTypes();
			bindDataGenericProperties(true);
			RaiseBubbleEvent(new object(),new SaveClickEventArgs("Saved"));
		}

		#region "Info" Pane


		private void setupInfoPane() 
		{
			InfoTabPage = TabView1.NewTabPage("Info");
			InfoTabPage.Controls.Add(pnlInfo);

			InfoTabPage.Style.Add("text-align","center");
			
			ImageButton Save = InfoTabPage.Menu.NewImageButton();
			Save.Click += new System.Web.UI.ImageClickEventHandler(save_click);
			
			Save.ImageUrl = UmbracoPath + "/images/editor/save.gif";
			Save.AlternateText = ui.Text("save");

            // Get icons
            DirectoryInfo dirInfo = new DirectoryInfo(this.Page.Server.MapPath(GlobalSettings.Path + "/images/umbraco"));
            FileInfo[] fileInfo = dirInfo.GetFiles();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                ListItem li = new ListItem(fileInfo[i].Name);
                if (!this.Page.IsPostBack && li.Value == cType.IconUrl) li.Selected = true;
                ddlIcons.Items.Add(li);
            }

            // Get thumbnails
            dirInfo = new DirectoryInfo(this.Page.Server.MapPath(GlobalSettings.Path + "/images/thumbnails"));
            fileInfo = dirInfo.GetFiles();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                ListItem li = new ListItem(fileInfo[i].Name);
                if (!this.Page.IsPostBack && li.Value == cType.Thumbnail) li.Selected = true;
                ddlThumbnails.Items.Add(li);
            }

            txtName.Text = cType.GetRawText();
			txtAlias.Text = cType.Alias;
		    description.Text = cType.Description;

		}
		#endregion


		#region "Structure" Pane
		private void setupStructurePane() 
		{
			dualAllowedContentTypes.ID = "allowedContentTypes";
			dualAllowedContentTypes.Width = 175;
			
			uicontrols.TabPage tp =  TabView1.NewTabPage("Structure");
			tp.Controls.Add(pnlStructure);
			tp.Style.Add("text-align","center");
			ImageButton Save = tp.Menu.NewImageButton();
			Save.Click += new System.Web.UI.ImageClickEventHandler(save_click);
			Save.ImageUrl = UmbracoPath + "/images/editor/save.gif";
			
			int[] allowedIds = cType.AllowedChildContentTypeIDs;
			if (!Page.IsPostBack) 
			{
				string chosenContentTypeIDs = "";
				foreach (cms.businesslogic.ContentType ct in cType.GetAll()) 
				{
					ListItem li = new ListItem(ct.Text,ct.Id.ToString());
					dualAllowedContentTypes.Items.Add(li);
					lstAllowedContentTypes.Items.Add(li);
					foreach (int i in allowedIds) 
						if (i == ct.Id) 
						{
							li.Selected= true;
							chosenContentTypeIDs += ct.Id + ",";
						}
				}
				dualAllowedContentTypes.Value = chosenContentTypeIDs;
			}
//			PlaceHolderAllowedContentTypes.Controls.Add(dualAllowedContentTypes);
		}

		private void SaveAllowedChildTypes() {
			ArrayList tmp = new ArrayList();
			foreach (ListItem li in lstAllowedContentTypes.Items) 
			{
				if (li.Selected)
					tmp.Add(int.Parse(li.Value));
			}
			int[] ids = new int[tmp.Count];
			for (int i = 0;i <tmp.Count;i++) ids[i] = (int) tmp[i];
			cType.AllowedChildContentTypeIDs = ids;
		}

		#endregion

		#region "Generic properties" Pane
		private void setupGenericPropertiesPane() 
		{
			uicontrols.TabPage tp =  TabView1.NewTabPage("Generic properties");
			tp.Style.Add("text-align","center");
			tp.Controls.Add(pnlProperties);
			
			ImageButton Save = tp.Menu.NewImageButton();
			Save.Click += new System.Web.UI.ImageClickEventHandler(save_click);
			Save.ImageUrl = UmbracoPath + "/images/editor/save.gif";

			dlTabs.ItemCommand += new DataListCommandEventHandler(dlTabs_ItemCommand);
			bindDataGenericProperties(false);
		}

		protected void dgTabs_itemdatabound(object sender,DataGridItemEventArgs e) {
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
			{
				((DropDownList)e.Item.FindControl("dllTab")).SelectedValue =
					((DataRowView)e.Item.DataItem).Row["tabid"].ToString();
				((DropDownList)e.Item.FindControl("ddlType")).SelectedValue =
					((DataRowView)e.Item.DataItem).Row["type"].ToString();
			}
			
		}
		private void bindDataGenericProperties(bool Refresh) 
		{
			cms.businesslogic.ContentType.TabI[] tabs = cType.getVirtualTabs;
			cms.businesslogic.datatype.DataTypeDefinition[] dtds = cms.businesslogic.datatype.DataTypeDefinition.GetAll();

			PropertyTypes.Controls.Clear();

			// Add new property
			if (PropertyTypeNew.Controls.Count == 0) 
			{
				PropertyTypeNew.Controls.Add(new LiteralControl("<h2 style=\"margin-top: 0px; margin-left: 0px;\">Add New Property</h2><div style=\"margin-left: -16px;\">"));
				gp = new controls.GenericProperties.GenericPropertyWrapper();
				gp.ID = "GenericPropertyNew";
				gp.Tabs = tabs;
				gp.DataTypeDefinitions = dtds;
				PropertyTypeNew.Controls.Add(gp);
				PropertyTypeNew.Controls.Add(new LiteralControl("</div>"));
			} 
			else if (Refresh) 
			{
                gp = (controls.GenericProperties.GenericPropertyWrapper) PropertyTypeNew.Controls[1];
				gp.ID = "GenericPropertyNew";
				gp.Tabs = tabs;
				gp.DataTypeDefinitions = dtds;
				gp.UpdateEditControl();
				gp.GenricPropertyControl.UpdateInterface();
				gp.GenricPropertyControl.Clear();
			}

			_genericProperties.Clear();
			Hashtable inTab = new Hashtable(); 
			int counter = 0;
            foreach (cms.businesslogic.ContentType.TabI t in tabs) 
			{
				PropertyTypes.Controls.Add(new LiteralControl("<h2 style=\"margin-left: 0px;\">Tab: " + t.GetRawCaption() + "</h2>"));

				if (t.PropertyTypes.Length > 0) 
				{
					HtmlInputHidden propSort = new HtmlInputHidden();
                    propSort.ID = "propSort_" + t.GetRawCaption().Replace(" ", "").Replace("#", "_") + "Content";
					PropertyTypes.Controls.Add(propSort);
					_sortLists.Add(propSort);
                    PropertyTypes.Controls.Add(new LiteralControl("<ul id=\"" + t.GetRawCaption().Replace(" ", "").Replace("#", "_") + "Contents\">"));		

					foreach (cms.businesslogic.propertytype.PropertyType pt in t.PropertyTypes) 
					{
						GenericProperties.GenericPropertyWrapper gpw = new umbraco.controls.GenericProperties.GenericPropertyWrapper();

                        // Changed by duckie, was:
                        // gpw.ID = "gpw_" + pt.Alias;
                        // Which is NOT unique!
                        gpw.ID = "gpw_" + pt.Id;

                        gpw.PropertyType = pt;
						gpw.Tabs = tabs;
						gpw.TabId = t.Id;
						gpw.DataTypeDefinitions = dtds;
						gpw.Delete += new EventHandler(gpw_Delete);
                        gpw.FullId = t.GetRawCaption().Replace(" ", "").Replace("#", "_") + "Contents_" + +pt.Id;
						PropertyTypes.Controls.Add(gpw);
						_genericProperties.Add(gpw);
						if (Refresh)
							gpw.GenricPropertyControl.UpdateInterface();
						inTab.Add(pt.Id.ToString(), "");
						counter++;
					}
					PropertyTypes.Controls.Add(new LiteralControl("</ul>"));
                    PropertyTypes.Controls.Add(new LiteralControl("<script>\n Sortable.create(\"" + t.GetRawCaption().Replace(" ", "").Replace("#", "_") + "Contents\",{onUpdate:function(element) {document.getElementById('" + propSort.ClientID + "').value = Sortable.serialize('" + t.GetRawCaption().Replace(" ", "").Replace("#", "_") + "Contents');},dropOnEmpty:false,containment:[\"" + t.GetRawCaption().Replace(" ", "").Replace("#", "_") + "Contents\"],constraint:'vertical'});\n</script>"));
				} 
				else 
				{
					PropertyTypes.Controls.Add(new LiteralControl("<p style=\"margin: 0px;\">No properties defined on this tab. Click on the \"add a new property\" link at the top to create a new property.</p>"));		
				}

			}

			// Generic properties tab
			counter = 0;
			bool propertyTabHasProperties = false;
			PlaceHolder propertiesPH = new PlaceHolder();
			propertiesPH.ID = "propertiesPH";
			PropertyTypes.Controls.Add(new LiteralControl("<h2 style=\"margin-left: 0px;\">Tab: Generic Properties</h2>"));
			PropertyTypes.Controls.Add(propertiesPH);

			HtmlInputHidden propSort_gp = new HtmlInputHidden();
			propSort_gp.ID = "propSort_generalPropertiesContent";
			propertiesPH.Controls.Add(propSort_gp);
			_sortLists.Add(propSort_gp);


			propertiesPH.Controls.Add(new LiteralControl("<ul id=\"generalPropertiesContents\">"));
			foreach (cms.businesslogic.propertytype.PropertyType pt in cType.PropertyTypes) 
			{
				if (!inTab.ContainsKey(pt.Id.ToString()))
				{
					GenericProperties.GenericPropertyWrapper gpw = new umbraco.controls.GenericProperties.GenericPropertyWrapper();

                    // Changed by duckie, was:
                    // gpw.ID = "gpw_" + pt.Alias;
                    // Which is NOT unique!
                    gpw.ID = "gpw_" + pt.Id;

					gpw.PropertyType = pt;
					gpw.Tabs = tabs;
					gpw.DataTypeDefinitions = dtds;
					gpw.Delete += new EventHandler(gpw_Delete);
					gpw.FullId = "generalPropertiesContents_" + pt.Id;

					propertiesPH.Controls.Add(gpw);
					_genericProperties.Add(gpw);
					if (Refresh)
						gpw.GenricPropertyControl.UpdateInterface();
					inTab.Add(pt.Id, "");
					propertyTabHasProperties = true;
					counter++;
				}
			}
			propertiesPH.Controls.Add(new LiteralControl("</ul>"));
			propertiesPH.Controls.Add(new LiteralControl("<script>\n Sortable.create(\"generalPropertiesContents\",{dropOnEmpty:false,containment:[\"generalPropertiesContents\"],constraint:'vertical',onUpdate:function(element) {document.getElementById('" + propSort_gp.ClientID + "').value = Sortable.serialize('generalPropertiesContents');}});\n</script>"));
			if (!propertyTabHasProperties) 
			{
				PropertyTypes.Controls.Add(new LiteralControl("<p style=\"margin: 0px;\">No properties defined on this tab. Click on the \"add a new property\" link at the top to create a new property.</p>"));		
				PropertyTypes.Controls.Remove(PropertyTypes.FindControl("propertiesPH"));
			}
			else
				PropertyTypes.Controls.Add(propertiesPH);

		}

		protected void gpw_Delete(object sender, System.EventArgs e) 
		{
			GenericProperties.GenericPropertyWrapper gpw = (GenericProperties.GenericPropertyWrapper) sender;
			gpw.GenricPropertyControl.PropertyType.delete();
			this.bindDataGenericProperties(true);
		}
		

		private void _old_bindDataGenericProperties() 
		{

			// Data bind create new
			gp.GenricPropertyControl.Tabs = cType.getVirtualTabs;
			gp.GenricPropertyControl.DataTypeDefinitions = cms.businesslogic.datatype.DataTypeDefinition.GetAll();

			DataSet ds = new DataSet();

			DataTable dtP = new DataTable("Properties");
			DataTable dtT = new DataTable("Tabs");
				
			ds.Tables.Add(dtP);
			ds.Tables.Add(dtT);

			dtP.Columns.Add("id");
			dtP.Columns.Add("tabid");
			dtP.Columns.Add("alias");
			dtP.Columns.Add("name");
			dtP.Columns.Add("type");
			dtP.Columns.Add("tab");

			dtT.Columns.Add("tabid");
			dtT.Columns.Add("TabName");
			dtT.Columns.Add("genericProperties");

			Hashtable inTab = new Hashtable(); 
			foreach (cms.businesslogic.ContentType.TabI tb in cType.getVirtualTabs)
			{
				DataRow dr = dtT.NewRow();
				dr["TabName"] = tb.GetRawCaption();
				dr["tabid"] = tb.Id;
				dtT.Rows.Add(dr);

				foreach (cms.businesslogic.propertytype.PropertyType pt in tb.PropertyTypes) 
				{
					DataRow dr1 = dtP.NewRow();
					dr1["alias"] = pt.Alias;
					dr1["tabid"] = tb.Id;
					dr1["name"] = pt.GetRawName();
					dr1["type"] = pt.DataTypeDefinition.Id;
					dr1["tab"] = tb.GetRawCaption();
					dr1["id"] = pt.Id.ToString();
					dtP.Rows.Add(dr1);
					inTab.Add(pt.Id.ToString(), true);
				}
			}

			DataRow dr2 = dtT.NewRow();
			dr2["TabName"] = "General properties";
			dr2["tabid"] = 0;
			dtT.Rows.Add(dr2);

			foreach (cms.businesslogic.propertytype.PropertyType pt in cType.PropertyTypes) 
			{
				if (!inTab.ContainsKey(pt.Id.ToString())) 
				{
					DataRow dr1 = dtP.NewRow();
					dr1["alias"] = pt.Alias;
					dr1["tabid"] = 0;
					dr1["name"] = pt.GetRawName();
					dr1["type"] = pt.DataTypeDefinition.Id;
					dr1["tab"] = "General properties";
					dr1["id"] = pt.Id.ToString();
					dtP.Rows.Add(dr1);
				}
			}

		
			ds.Relations.Add(new DataRelation("tabidrelation", dtT.Columns["tabid"],dtP.Columns["tabid"],false));

			dlTabs.DataSource = ds.Tables["Tabs"].DefaultView;
			dlTabs.DataBind();

/*			ddlPropertyTab.DataSource = TabTable;
			ddlPropertyTab.DataTextField = "name";
			ddlPropertyTab.DataValueField = "id";
			ddlPropertyTab.DataBind();

			ddlPropertyType.DataSource = DataTypeTable;
			ddlPropertyType.DataTextField = "name";
			ddlPropertyType.DataValueField = "id";
			ddlPropertyType.DataBind();

*/		}

		
		private void saveProperties() {
			this.CreateChildControls();

			GenericProperties.GenericProperty gpData = gp.GenricPropertyControl;
			if (gpData.Name.Trim() != "" && gpData.Alias.Trim() != "") 
			{
				string[] info = {gpData.Name, gpData.Type.ToString()};
				cType.AddPropertyType(cms.businesslogic.datatype.DataTypeDefinition.GetDataTypeDefinition(gpData.Type),gpData.Alias,gpData.Name);
				cms.businesslogic.propertytype.PropertyType pt =  cType.getPropertyType(gpData.Alias);
				pt.Mandatory = gpData.Mandatory;
				pt.ValidationRegExp = gpData.Validation;
				pt.Description = gpData.Description;

				if (gpData.Tab != 0) 
				{
					cType.SetTabOnPropertyType(cType.getPropertyType(gpData.Alias),gpData.Tab);
				}
				gpData.Clear() ;

			} 

			foreach(GenericProperties.GenericPropertyWrapper gpw in _genericProperties) 
			{
				cms.businesslogic.propertytype.PropertyType pt = gpw.PropertyType;
				pt.Alias = gpw.GenricPropertyControl.Alias;
				pt.Name = gpw.GenricPropertyControl.Name;
				pt.Description = gpw.GenricPropertyControl.Description;
				pt.ValidationRegExp = gpw.GenricPropertyControl.Validation;
				pt.Mandatory = gpw.GenricPropertyControl.Mandatory;
				pt.DataTypeDefinition = cms.businesslogic.datatype.DataTypeDefinition.GetDataTypeDefinition(gpw.GenricPropertyControl.Type);
				if (gpw.GenricPropertyControl.Tab == 0)
					cType.removePropertyTypeFromTab(pt);
				else
					cType.SetTabOnPropertyType(pt, gpw.GenricPropertyControl.Tab);
                
                pt.Save();
			}

			// Sort order
			foreach(HtmlInputHidden propSorter in _sortLists) 
			{
				if (propSorter.Value.Trim() != "") 
				{
					string tabName = propSorter.ID.Replace("propSort_", "");
					tabName = tabName.Substring(0, tabName.Length-7);
				
					string[] tempSO = propSorter.Value.Split("&".ToCharArray());
					for(int i=0;i<tempSO.Length;i++) 
					{
						int currentSortOrder = int.Parse(tempSO[i].Replace(tabName + "Contents[]=", ""));
						cms.businesslogic.propertytype.PropertyType.GetPropertyType(currentSortOrder).SortOrder = i;
					}
				}
			}




			/*

			foreach (DataListItem dli in dlTabs.Items)
			{
				DataGrid dg = (DataGrid)dli.FindControl("dgGenericPropertiesOfTab");
				foreach (DataGridItem dgi in dg.Items) 
				{
					
					int propertyTypeId = int.Parse(dgi.Cells[0].Text);
					cms.businesslogic.propertytype.PropertyType pt = new cms.businesslogic.propertytype.PropertyType(propertyTypeId);
					string tbName = ((TextBox) dgi.FindControl("txtPName")).Text.Replace("'","''");
					string tbAlias = ((TextBox) dgi.FindControl("txtPAlias")).Text.Replace("'","''");
					int TypeId = int.Parse(((DropDownList) dgi.FindControl("ddlType")).SelectedValue);
					int TabId = int.Parse(((DropDownList) dgi.FindControl("dllTab")).SelectedValue);
				
					if (TypeId != pt.DataTypeDefinition.Id) 
						pt.DataTypeDefinition = new cms.businesslogic.datatype.DataTypeDefinition(TypeId);
					
                    if (tbName != pt.Name) 
						pt.Name = tbName;
					
					if (tbAlias != pt.Alias) 
						pt.Alias = tbAlias;
					if (TabId == 0)
						cType.removePropertyTypeFromTab(pt);
					else
						cType.SetTabOnPropertyType(pt,TabId);
				}
				
			}
			*/
		}

		public bool HasRows(System.Data.DataView dv) {
			return (dv.Count == 0);
		}
		private void dlTabs_ItemCommand(object source, DataListCommandEventArgs e)
		{
			if (e.CommandName == "Delete") {
				cType.DeleteVirtualTab(int.Parse(e.CommandArgument.ToString()));
			}

			if (e.CommandName == "MoveDown") 
			{
				int TabId = int.Parse(e.CommandArgument.ToString());
				foreach (cms.businesslogic.ContentType.TabI t in cType.getVirtualTabs)
				{
					if (t.Id == TabId) 
					{
						t.MoveDown();
					}
				}
			}

			if (e.CommandName == "MoveUp") 
			{
				int TabId = int.Parse(e.CommandArgument.ToString());
				foreach (cms.businesslogic.ContentType.TabI t in cType.getVirtualTabs)
				{
					if (t.Id == TabId) 
					{
						t.MoveUp();
					}
				}
			}
			bindTabs();
			bindDataGenericProperties(false);
		}

		
		protected void dgGenericPropertiesOfTab_itemcommand(object sender, DataGridCommandEventArgs e) 
		{
			// Delete propertytype from contenttype
			if (e.CommandName == "Delete") 
			{
				int propertyId = int.Parse(e.Item.Cells[0].Text);
				cms.businesslogic.propertytype.PropertyType pt = cms.businesslogic.propertytype.PropertyType.GetPropertyType(propertyId);
				RaiseBubbleEvent(new object(),new SaveClickEventArgs("Property �" +  pt.GetRawName() + "� deleted"));
				pt.delete();
				bindDataGenericProperties(false);			
			}
		}

		protected void dlTab_itemdatabound(object sender, DataListItemEventArgs e) {
			if (int.Parse(((DataRowView)e.Item.DataItem).Row["tabid"].ToString()) == 0) 
			{
				((Button)e.Item.FindControl("btnTabDelete")).Visible=false;
					((Button)e.Item.FindControl("btnTabUp")).Visible=false;
					((Button)e.Item.FindControl("btnTabDown")).Visible=false;
			}
		}
		

		#endregion

		#region "Tab" Pane
		private void setupTabPane() 
		{
			uicontrols.TabPage tp =  TabView1.NewTabPage("Tabs");
			
			
			pnlTab.Style.Add("text-align","center");
			tp.Controls.Add(pnlTab);
			ImageButton Save = tp.Menu.NewImageButton();
			Save.Click += new System.Web.UI.ImageClickEventHandler(save_click);
			Save.ID = "SaveButton";
			Save.ImageUrl = UmbracoPath + "/images/editor/save.gif";
			bindTabs();
		}

		private void bindTabs() 
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("name");
			dt.Columns.Add("id");
			foreach (cms.businesslogic.ContentType.TabI tb in cType.getVirtualTabs)
			{
				DataRow dr = dt.NewRow();
				dr["name"] = tb.GetRawCaption();
				dr["id"] = tb.Id;
				dt.Rows.Add(dr);
			}


			if (dt.Rows.Count ==0) 
			{
				lttNoTabs.Text = "No custom tabs defined";
				dgTabs.Visible = false;
			} 
			else {
				lttNoTabs.Text = "";
				dgTabs.Visible = true;
			}
			dgTabs.DataSource = dt;
			dgTabs.DataBind();
		}

		public DataTable TabTable {
			get {
                if (dgTabs.DataSource == null) 
					bindTabs();

				DataTable dt = new DataTable();
				dt.Columns.Add("name");
				dt.Columns.Add("id");
				
				foreach (DataRow dr in ((DataTable)dgTabs.DataSource).Rows) {
					DataRow dr2 = dt.NewRow();
					dr2["name"] = dr["name"];
					dr2["id"] = dr["id"];
					dt.Rows.Add(dr2);
				}

				DataRow dr1 = dt.NewRow();
				dr1["name"] = "General properties";
				dr1["id"] = 0;
				dt.Rows.Add(dr1);

				return dt;
			}
		}
		
		private DataTable _DataTypeTable;

		public DataTable DataTypeTable 
		{
			get 
			{
				if (_DataTypeTable == null) 
				{
					_DataTypeTable = new DataTable();
					_DataTypeTable.Columns.Add("name");
					_DataTypeTable.Columns.Add("id");
					foreach(cms.businesslogic.datatype.DataTypeDefinition DataType in cms.businesslogic.datatype.DataTypeDefinition.GetAll()) 
					{
						DataRow dr = _DataTypeTable.NewRow();
						dr["name"] = DataType.Text;
						dr["id"] = DataType.Id.ToString();
						_DataTypeTable.Rows.Add(dr);
					}
				}
				return _DataTypeTable;
			}
		}
	
		
		protected void btnNewTab_Click(object sender, System.EventArgs e)
		{
			if (txtNewTab.Text.Trim() != "") 
			{
				cType.AddVirtualTab(txtNewTab.Text);
				RaiseBubbleEvent(new object(),new SaveClickEventArgs("Tab �" +  txtNewTab.Text + "� created"));
				txtNewTab.Text ="";
				bindTabs();
				bindDataGenericProperties(true);
			}
		}

		protected void dgTabs_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete") 
			{
				cType.DeleteVirtualTab(int.Parse(e.Item.Cells[0].Text));
			}
			
		
			bindTabs();
			bindDataGenericProperties(true);
		}

		

		private void SaveTabs() {
			foreach (DataGridItem dgi in dgTabs.Items) {
				int tabid = int.Parse(dgi.Cells[0].Text);
				string tabName = ((TextBox)dgi.FindControl("txtTab")).Text.Replace("'","''");
				cType.SetTabName(tabid,tabName);
			}
		}

		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);

			int docTypeId = int.Parse(Request.QueryString["id"]);
			cType = new cms.businesslogic.ContentType(docTypeId);
			
			setupInfoPane();
			setupStructurePane();
			setupGenericPropertiesPane();
			setupTabPane();
			
		}
		
		private void InitializeComponent()
		{

		}
		#endregion
	}

	public class SaveClickEventArgs : EventArgs
	{
		private string _message = "";
		public string Message { 
			get {return _message;}	
		}
		public SaveClickEventArgs(string message) {
			_message = message;
		}
  }
}