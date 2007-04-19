using System;
using System.IO;
using System.Xml;
using System.CodeDom.Compiler;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Serialization;

namespace CBinding
{
	public enum Language {
		C,
		CPP
	};
	
	[DataInclude(typeof(CProjectConfiguration))]
	public class CProject : Project
	{
		[ItemProperty("Compiler")]
		private ICompiler compiler_manager;
		
		[ItemProperty("language")]
		private Language language = Language.C;
		
		public CProject ()
		{
		}
		
		public CProject (ProjectCreateInformation info, XmlElement projectOptions)
		{
			string binPath = ".";
			
			if (info != null) {
				Name = info.ProjectName;
				binPath = info.BinPath;
			}
			
			
			if (projectOptions != null) {
				if (projectOptions.Attributes["Language"] != null) {
					language = (Language)Enum.Parse (
						typeof(Language),
						projectOptions.Attributes["Language"].InnerText);
				}
			}
			
			Compiler = null; // use default compiler depending on language
			
			CProjectConfiguration configuration =
				(CProjectConfiguration)CreateConfiguration ("Debug");
				
			Configurations.Add (configuration);
			
			configuration =
				(CProjectConfiguration)CreateConfiguration ("Release");
				
			configuration.DebugMode = false;
			Configurations.Add (configuration);
			
			foreach (CProjectConfiguration c in Configurations) {
				c.OutputDirectory = Path.Combine (binPath, c.Name);
				c.Output = Name;
				
				if (projectOptions != null) {
					if (projectOptions.Attributes["Target"] != null) {
						c.CompileTarget = (CBinding.CompileTarget)Enum.Parse (
						     typeof(CBinding.CompileTarget),
							 projectOptions.Attributes["Target"].InnerText);
					}
					if (projectOptions.Attributes["PauseConsoleOutput"] != null) {
						c.PauseConsoleOutput = bool.Parse (
							projectOptions.Attributes["PauseConsoleOutput"].InnerText);
					}
				}
			}			
		}
		
		public override string ProjectType {
			get { return "C/C++"; }
		}
		
		public override string[] SupportedLanguages {
			get { return new string[] { "C", "C++" }; }
		}
		
		public override bool IsCompileable (string fileName)
		{
			if (language == Language.C) {
				return (Path.GetExtension (fileName.ToUpper ()) == ".C");
			} else {
				return (Path.GetExtension (fileName.ToUpper ()) == ".CPP");
			}
		}
		
		protected override ICompilerResult DoBuild (IProgressMonitor monitor)
		{
			return compiler_manager.Compile (ProjectFiles,
			                        ProjectReferences,
			                        (CProjectConfiguration)ActiveConfiguration,
			                        monitor);
		}
		
		protected override void DoExecute (IProgressMonitor monitor,
		                                   ExecutionContext context)
		{
			// TODO: implement
		}
		
		public override string GetOutputFileName ()
		{
			CProjectConfiguration conf = (CProjectConfiguration)ActiveConfiguration;
			return conf.Name;
		}
		
		public override IConfiguration CreateConfiguration (string name)
		{
			CProjectConfiguration conf = new CProjectConfiguration ();
			
			conf.Name = name;
			conf.CompilationParameters = new CCompilationParameters ();
			
			return conf;
		}
		
		public Language Language {
			get { return language; }
		}
		
		public ICompiler Compiler {
			set {
				if (value != null) {
					compiler_manager = value;
				} else {
					if (language == Language.C)
						compiler_manager = new GccCompiler ();
					//else
						// TODO: compiler_manager = new GppCompiler ();
				}
			}
		}
	}
}
