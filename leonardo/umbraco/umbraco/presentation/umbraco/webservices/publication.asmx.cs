using System;
using System.ComponentModel;
using System.Web.Services;

namespace umbraco.webservices
{
	/// <summary>
	/// Summary description for publication.
	/// </summary>
	[WebService(Namespace="http://umbraco.org/webservices/")]
	public class publication : WebService
	{
		public publication()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		[WebMethod]
		public int GetPublicationStatus(string key) 
		{
			try 
			{
				return int.Parse(Application["publishDone" + key].ToString());
			} 
			catch 
			{
				return 0;
			}
		}

		[WebMethod]
		public int GetPublicationStatusMax(string key) 
		{
			try 
			{
				return int.Parse(Application["publishTotal" + key].ToString());
			} 
			catch 
			{
				return 0;
			}
		}

		[WebMethod]
		public void HandleReleaseAndExpireDates(Guid PublishingServiceKey) 
		{
		}

        [WebMethod]
        public void SaveXmlCacheToDisk()
        {
			content.Instance.SaveContentToDisk(content.Instance.XmlContent);
        }

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

	}
}
