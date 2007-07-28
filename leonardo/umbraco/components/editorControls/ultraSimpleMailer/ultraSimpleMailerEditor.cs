using System;
using System.Collections;

using umbraco.editorControls.wysiwyg;
using umbraco.uicontrols;

namespace umbraco.editorControls.ultraSimpleMailer
{
	/// <summary>
	/// Summary description for ultraSimpleMailerEditor.
	/// </summary>
	public class ultraSimpleMailerEditor : umbraco.editorControls.wysiwyg.editor
	{
        umbraco.cms.businesslogic.datatype.DefaultData _data;
		string _configuration;
		private controls.progressBar pb;

        public ultraSimpleMailerEditor(umbraco.cms.businesslogic.datatype.DefaultData Data, string Configuration)
            : base(Data)
		{
			_configuration = Configuration;
			_data = Data;
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// init progressbar
			pb = new umbraco.controls.progressBar();
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UltraSimpleMailerJs", "<script language='javascript' src='/umbraco_client/ultraSimpleMailer/javascript.js'></script>");
			string[] config = _configuration.Split("|".ToCharArray());
			cms.businesslogic.member.MemberGroup mg = new umbraco.cms.businesslogic.member.MemberGroup(int.Parse(config[2]));
			string totalReceip = mailerLogic.GetTotalReceiptients(mg).ToString();
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ultraSimpleMailerAjax", "<script language='javascript' src='webservices/ajax.js'></script><script language='javascript' src='webservices/GetJavaScriptProxy.aspx?service=progressStatus.asmx'></script><script>\nultraSimpleMailerTotalMails = " + totalReceip + ";\nvar ultraSimpleMailerId = 'ultraSimpleMailerProgress" + this._data.NodeId + "';</script>");
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "progressBar", "<script language='javascript' src='/umbraco_client/progressBar/javascript.js'></script>");
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "progressBarCss", "<LINK href=\"/umbraco_client/progressBar/style.css\" type=\"text/css\" rel=\"stylesheet\">");
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{

			// Debug - needs logic
			string[] config = _configuration.Split("|".ToCharArray());
			cms.businesslogic.member.MemberGroup mg = new umbraco.cms.businesslogic.member.MemberGroup(int.Parse(config[2]));

			writer.WriteLine("<input type=\"hidden\" name=\"" + this.ClientID + "_doSend\" id=\"" + this.ClientID + "_doSend\" value=\"" + umbraco.helper.Request(this.ClientID + "_doSend") + "\" />");
			writer.WriteLine("<input type=\"hidden\" name=\"" + this.ClientID + "_doTest\" id=\"" + this.ClientID + "_doTest\" value=\"" + umbraco.helper.Request(this.ClientID + "_doTest") + "\" />");

			if (umbraco.helper.Request(this.ClientID + "_doTest") == "" && umbraco.helper.Request(this.ClientID + "_doSend") == "")
				base.Render (writer);
			else 
			{
				writer.WriteLine("<div class=\"propertypane\" style=\"margin: 10px; padding: 10px; width: 100%;height: 100%;background-color: light-blue\">");

				if (umbraco.helper.Request(this.ClientID + "_sendButton") != "") 
				{
					// Test mail
					if (umbraco.helper.Request(this.ClientID + "_doTest") != "") 
					{
						writer.WriteLine("<h3 style=\"margin-left: -1px;\">Send newsletter to test...</h3><br/>");
						mailerLogic.SendTestmail(umbraco.helper.Request(this.ClientID + "_test_rcp"), new cms.businesslogic.property.Property(_data.PropertyId), config[0], config[1], true);
						writer.WriteLine("Test mail sent to: <b>" + umbraco.helper.Request(this.ClientID + "_test_rcp") + "</b><br/>");
					} 
					else 
					{
						writer.WriteLine("<h3 style=\"margin-left: -1px;\">Send newsletter to all...</h3><br/>");
						mailerLogic.SendMail(mg, new cms.businesslogic.property.Property(_data.PropertyId), config[0], config[1], true);
						writer.WriteLine("Sent...<br/>");

						}
				} 
				else 
				{
					if (umbraco.helper.Request(this.ClientID + "_doTest") != "")  
					{
						writer.WriteLine("<h3 style=\"margin-left: -1px;\">Send newsletter to test...</h3><br/>");
						writer.WriteLine("Send test to: <input type=\"text\" name=\"" + this.ClientID + "_test_rcp\" id=\"" + this.ClientID + "_test_rcp\"/>");
						writer.WriteLine("<input type=\"submit\" name=\"" + this.ClientID + "_sendButton\" class=\"guiInputButton\" value=\"Send\"/>");
					}
					else 
					{
						writer.WriteLine(
						"			<div id=\"ultraSimpleMailerAnimDiv\" style=\"DISPLAY: none; TEXT-ALIGN: left\"><br /><br /><span style=\"border: #999 1px solid; PADDING: 15px; BACKGROUND: white; WIDTH: 300px; TEXT-ALIGN: center\"><img id=\"ultraSimpleMailerAnim\" height=\"42\" alt=\"Sending mails...\" src=\"images/anims/publishPages.gif\"" + 
						"			width=\"150\" /><br />\n" + 
						"		<script>\n" + 
						"		umbPgStep = 1;\n" + 
						"		umbPgIgnoreSteps = true;\n" + 
						"		</script>\n" + 
						"		<span class=\"guiDialogTiny\" style=\"TEXT-ALIGN: center\">Sending mails...</span>\n" + 
						"		<br />\n" + 
						"		<br />\n");

						// Progressbar
						pb.ID = "ultraSimpleMailerUpgradeStatus";
						pb.Width = 200;
						pb.RenderControl(writer);
						writer.WriteLine("		</span><br /><br /></div>\n" + 
						"<div id=\"ultraSimpleMailerFormDiv\">");
						writer.WriteLine("<h3 style=\"margin-left: -1px;\">Send newsletter to all...</h3><br/>");
						writer.WriteLine("Please confirm that you want to send this message to <b>" + mailerLogic.GetTotalReceiptients(mg).ToString() + "</b> recipients<br/>");
						writer.WriteLine("<input type=\"checkbox\" name=\"" + this.ClientID + "_sendButton\" id=\"" + this.ClientID + "_sendButton\" value=\"1\"/> Yes<br/>");
						writer.WriteLine("<input style=\"margin-left: -10px;\" type=\"button\" onClick=\"if (document.getElementById('" + this.ClientID + "_sendButton').checked) {ultraSimpleMailerDoSend('" + this.ClientID + "');} else {alert('Please confirm');}\" value=\"Send\"/>");
						writer.WriteLine("</div>");
					}

				}
				writer.WriteLine("</div>");
			}

		}


		
		public override object[] MenuIcons
		{
			get
			{
				object[] _buttons = base.MenuIcons;

				ArrayList buttons = new ArrayList();
				for (int i=0;i<_buttons.Length;i++)
					buttons.Add(_buttons[i]);

				// Add the two new buttons
				MenuIconI menuItemSend = new MenuIconClass();						
				menuItemSend.OnClickCommand = "ultraSimpleMailer_doSend('" + this.ClientID + "')";
				menuItemSend.ImageURL = "/umbraco_client/ultraSimpleMailer/images/newsletterSend.gif";
				menuItemSend.AltText = "Send newsletter to all";
				menuItemSend.ID = "sendToAll";
				buttons.Insert(0, menuItemSend);
				
				MenuIconI menuItemTest = new MenuIconClass();						
				menuItemTest.OnClickCommand = "ultraSimpleMailer_doSendTest('" + this.ClientID + "')";
				menuItemTest.ImageURL = "/umbraco_client/ultraSimpleMailer/images/newsletterSendTest.gif";
				menuItemTest.AltText = "Test newsletter by sending to a mail address you specify";
				menuItemTest.ID = "sendToTest";
				buttons.Insert(1, menuItemTest);
				buttons.Insert(2, "|");

				// Re-create the button array
                _buttons = new object[buttons.Count];
				for (int i=0;i<buttons.Count;i++)
					_buttons[i] = buttons[i];


				return _buttons;
			}
		}


	}
}
