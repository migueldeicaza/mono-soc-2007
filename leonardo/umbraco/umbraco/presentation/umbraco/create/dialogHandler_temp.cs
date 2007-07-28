using System;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;

namespace umbraco.presentation.create
{
	/// <summary>
	/// Summary description for dialogHandler_temp.
	/// </summary>
	public class dialogHandler_temp
	{
		public dialogHandler_temp()
		{
		}

		public static void Delete(string NodeType, int NodeId) 
		{
			Delete(NodeType, NodeId, "");
		}
		public static void Delete(string NodeType, int NodeId, string Text) 
		{
			try 
			{
				// Load task settings
				XmlDocument createDef = new XmlDocument();
				XmlTextReader defReader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path+ "/config/create/UI.xml"));
				createDef.Load(defReader);
				defReader.Close();

				// Create an instance of the type by loading it from the assembly
				XmlNode def = createDef.SelectSingleNode("//nodeType [@alias = '" + NodeType + "']");
				string taskAssembly = def.SelectSingleNode("./tasks/delete").Attributes.GetNamedItem("assembly").Value;
				string taskType = def.SelectSingleNode("./tasks/delete").Attributes.GetNamedItem("type").Value;

				Assembly assembly = Assembly.LoadFrom(System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../bin/"+taskAssembly+".dll"));
				Type type = assembly.GetType(taskAssembly+"."+taskType);
				interfaces.ITask typeInstance = Activator.CreateInstance(type) as interfaces.ITask;
				if (typeInstance != null) 
				{
					typeInstance.ParentID = NodeId;
					typeInstance.Alias = Text;
					typeInstance.Delete();
				} 

			} 
			catch
			{
			}

		}

		public static string Create(string NodeType, int NodeId, string Text) 
		{
			return Create(NodeType, 0, NodeId, Text);
		}

		public static string Create(string NodeType, int TypeId, int NodeId, string Text) 
		{
			// Lets reflect...
			try 
			{
				// Load task settings
				XmlDocument createDef = new XmlDocument();
				XmlTextReader defReader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path+ "/config/create/UI.xml"));
				createDef.Load(defReader);
				defReader.Close();

				// Create an instance of the type by loading it from the assembly
				XmlNode def = createDef.SelectSingleNode("//nodeType [@alias = '" + NodeType + "']");
				string taskAssembly = def.SelectSingleNode("./tasks/create").Attributes.GetNamedItem("assembly").Value;
				string taskType = def.SelectSingleNode("./tasks/create").Attributes.GetNamedItem("type").Value;

				Assembly assembly = Assembly.LoadFrom(System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../bin/"+taskAssembly+".dll"));
				Type type = assembly.GetType(taskAssembly+"."+taskType);
				interfaces.ITask typeInstance = Activator.CreateInstance(type) as interfaces.ITask;
				if (typeInstance != null) 
				{
					typeInstance.TypeID = TypeId;
					typeInstance.ParentID = NodeId;
					typeInstance.Alias = Text;
					typeInstance.UserId = BasePages.UmbracoEnsuredPage.GetUserId(BasePages.UmbracoEnsuredPage.umbracoUserContextID);
					typeInstance.Save();

					// check for returning url
					try 
					{
						return ((interfaces.ITaskReturnUrl) typeInstance).ReturnUrl;
					} 
					catch 
					{
						return "";
					}
				} 

			} 
			catch
			{
				return "";
			}

			return "";
		}


	}
}
