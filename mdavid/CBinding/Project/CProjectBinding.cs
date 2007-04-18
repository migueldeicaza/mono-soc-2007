using System.Xml;
using MonoDevelop.Projects;

namespace CBinding
{
	public class CProjectBinding : IProjectBinding
	{
		public string Name {
			get { return "C/C++"; }
		}
		
		public Project CreateProject (ProjectCreateInformation info,
		                              XmlElement projectOptions)
		{
			return new CProject (info, projectOptions);
		}
		
		public Project CreateSingleFileProject (string sourceFile)
		{
			// TODO: implement
			return null;
		}
		
		public bool CanCreateSingleFileProject (string sourceFile)
		{
			// Not at the moment
			return false;
		}
	}
}
