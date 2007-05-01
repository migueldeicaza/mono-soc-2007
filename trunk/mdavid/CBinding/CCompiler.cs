using System.CodeDom.Compiler;

using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace CBinding
{
	public abstract class CCompiler
	{
		public abstract ICompilerResult Compile (
			ProjectFileCollection projectFiles,
		    ProjectReferenceCollection references,
		    CProjectConfiguration configuration,
		    IProgressMonitor monitor);
		
		public abstract string Name {
			get;
		}
			
		public abstract Language Language {
			get;
		}
		    
		protected abstract void ParseOutput (string errorString, CompilerResults cr);
	}
}
