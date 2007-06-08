//
// GNUCompiler.cs: Provides most functionality to compile using a GNU compiler (gcc and g++)
//
// Authors:
//   Marcos David Marin Amador <MarcosMarin@gmail.com>
//
// Copyright (C) 2007 Marcos David Marin Amador
//
//
// This source code is licenced under The MIT License:
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.CodeDom.Compiler;

using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Gui.Components;
using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui;

namespace CBinding
{
	public abstract class GNUCompiler : CCompiler
	{
		public override ICompilerResult Compile (
			ProjectFileCollection projectFiles,
		    ProjectPackageCollection packages,
		    CProjectConfiguration configuration,
		    IProgressMonitor monitor)
		{
			StringBuilder args = new StringBuilder ();
			CompilerResults cr = new CompilerResults (new TempFileCollection ());
			bool res = false;
			string outputName = configuration.OutputDirectory + "/" +
				configuration.CompiledOutputName;
			CCompilationParameters cp =
				(CCompilationParameters)configuration.CompilationParameters;
			
			switch (cp.WarningLevel)
			{
			case WarningLevel.None:
				args.Append ("-w ");
				break;
			case WarningLevel.Normal:
				// nothing
				break;
			case WarningLevel.All:
				args.Append ("-Wall ");
				break;
			}
			
			args.Append ("-O" + cp.OptimizationLevel + " ");
			
			if (cp.ExtraCompilerArguments != null && cp.ExtraCompilerArguments.Length > 0)
				args.Append (cp.ExtraCompilerArguments + " ");
			
			if (configuration.Includes != null)
				foreach (string inc in configuration.Includes)
					args.Append ("-I" + inc + " ");
			
			foreach (ProjectFile f in projectFiles) {
				if (f.BuildAction == BuildAction.Compile)
					res = DoCompilation (f, args.ToString (), monitor, cr);
				else
					res = true;
				
				if (!res) break;
			}
			
			if (res) {
				switch (configuration.CompileTarget)
				{
				case CBinding.CompileTarget.Bin:
					MakeBin (
						projectFiles, packages, configuration, cr, monitor, outputName);
					break;
				case CBinding.CompileTarget.StaticLibrary:
					MakeStaticLibrary (
						projectFiles, monitor, outputName);
					break;
				case CBinding.CompileTarget.SharedLibrary:
					MakeSharedLibrary (
						projectFiles, packages, configuration, cr, monitor, outputName);
					break;
				}
			}
			
			return new DefaultCompilerResult (cr, "");
		}
		
		private void MakeBin(ProjectFileCollection projectFiles,
		                     ProjectPackageCollection packages,
		                     CProjectConfiguration configuration,
		                     CompilerResults cr,
		                     IProgressMonitor monitor, string outputName)
		{
			string objectFiles = ObjectFiles (projectFiles);
			string pkgargs = GeneratePkgArgs (packages);
			StringBuilder args = new StringBuilder ();
			CCompilationParameters cp =
				(CCompilationParameters)configuration.CompilationParameters;
			
			if (cp.ExtraLinkerArguments != null && cp.ExtraLinkerArguments.Length > 0)
				args.Append (cp.ExtraLinkerArguments + " ");
			
			if (configuration.LibPaths != null)
				foreach (string libpath in configuration.LibPaths)
					args.Append ("-L" + libpath + " ");
			
			if (configuration.Libs != null)
				foreach (string lib in configuration.Libs)
					args.Append ("-l" + lib + " ");
			
			monitor.Log.WriteLine ("Generating binary...");
			
			// temp
			monitor.Log.WriteLine (
				"using: " + linkerCommand + " -o " + outputName + " " + objectFiles +
				" " + args.ToString () + " " + pkgargs);
			
			ProcessWrapper p = Runtime.ProcessService.StartProcess (
				linkerCommand, "-o " + outputName + " " + objectFiles + " " +
				args.ToString () + " " + pkgargs,
				null, null);
			p.WaitForExit ();
			
			StringBuilder error = new StringBuilder ();
			
			while (p.StandardError.EndOfStream == false)
				error.Append (p.StandardError.ReadLine () + "\n");
			
			ParseLinkerOutput (error.ToString (), cr);
		}
		
		private void MakeStaticLibrary (ProjectFileCollection projectFiles,
		                                IProgressMonitor monitor, string outputName)
		{
			string objectFiles = ObjectFiles (projectFiles);
			
			monitor.Log.WriteLine ("Generating static library...");
			
			Process p = Runtime.ProcessService.StartProcess (
				"ar", "rcs " + outputName + " " + objectFiles,
				null, null);
			p.WaitForExit ();
		}
		
		private void MakeSharedLibrary(ProjectFileCollection projectFiles,
		                               ProjectPackageCollection packages,
		                               CProjectConfiguration configuration,
		                               CompilerResults cr,
		                               IProgressMonitor monitor, string outputName)
		{
			string objectFiles = ObjectFiles (projectFiles);
			string pkgargs = GeneratePkgArgs (packages);
			StringBuilder args = new StringBuilder ();
			CCompilationParameters cp =
				(CCompilationParameters)configuration.CompilationParameters;
			
			if (cp.ExtraLinkerArguments != null && cp.ExtraLinkerArguments.Length > 0)
				args.Append (cp.ExtraLinkerArguments + " ");
			
			if (configuration.LibPaths != null)
				foreach (string libpath in configuration.LibPaths)
					args.Append ("-L" + libpath + " ");
			
			if (configuration.Libs != null)
				foreach (string lib in configuration.Libs)
					args.Append ("-l" + lib + " ");
			
			monitor.Log.WriteLine ("Generating shared object...");
			
			// temp
			monitor.Log.WriteLine (
				"using: " + linkerCommand + " -shared -o " + outputName + " " +
				objectFiles + " " + args.ToString () + " " + pkgargs);
			
			Process p = Runtime.ProcessService.StartProcess (
				linkerCommand, "-shared -o " + outputName + " " + objectFiles +
				" " + args.ToString () + " " + pkgargs,
				null, null);
			p.WaitForExit ();
			
			StringBuilder error = new StringBuilder ();
			
			while (p.StandardError.EndOfStream == false)
				error.Append (p.StandardError.ReadLine () + "\n");
			
			ParseLinkerOutput (error.ToString (), cr);
		}
		
		/// <summary>
		/// Compiles a source file into object code
		/// </summary>
		private bool DoCompilation (ProjectFile file, string args,
		                            IProgressMonitor monitor,
		                            CompilerResults cr)
		{			
			string outputName = Path.ChangeExtension (file.Name, ".o");
			
			monitor.Log.WriteLine (
				"using: " + compilerCommand + " " + file.Name + " " + args +
				"-c -o " + outputName);
			
			ProcessWrapper p = Runtime.ProcessService.StartProcess (
				compilerCommand, file.Name + " " + args + "-c -o " + outputName,
				null, null);
				
			p.WaitForExit ();
			
			StringBuilder error = new StringBuilder ();
			
			while (p.StandardError.EndOfStream == false)
				error.Append (p.StandardError.ReadLine () + "\n");
			
			ParseCompilerOutput (error.ToString (), cr);
			
			return p.ExitCode == 0;
		}
		
		private string ObjectFiles (ProjectFileCollection projectFiles)
		{
			StringBuilder objectFiles = new StringBuilder ();
			
			foreach (ProjectFile f in projectFiles) {
				if (f.BuildAction == BuildAction.Compile) {
					objectFiles.Append (Path.ChangeExtension (f.Name, ".o") + " ");
				}
			}
			
			return objectFiles.ToString ();
		}
		
		protected override void ParseCompilerOutput (string errorString, CompilerResults cr)
		{
			TextReader reader = new StringReader (errorString);
			string next;
			
			while ((next = reader.ReadLine ()) != null) {
				CompilerError error = CreateErrorFromErrorString (next);
				if (error != null)
					cr.Errors.Add (error);
			}
			
			reader.Close ();
		}
		
		private CompilerError CreateErrorFromErrorString (string errorString)
		{
			CompilerError error = new CompilerError ();
			string[] split;
			
			split = errorString.Split(new char[] { ':' });
			
			if (split.Length < 4)
				return null;
			
			if (split.Length == 4) {
				error.FileName = split[0];
				error.Line = int.Parse (split[1]);
				error.IsWarning = split[2].Equals ("warning");
				error.ErrorText = split[3];
			} else {
				error.FileName = split[0];
				error.Line = int.Parse (split[1]);
				error.Column = int.Parse (split[2]);
				error.IsWarning = split[3].Contains ("warning");
				error.ErrorText = split[4];
			}
			
			return error;
		}
		
		protected override void ParseLinkerOutput (string errorString, CompilerResults cr)
		{
			TextReader reader = new StringReader (errorString);
			string next;
			
			while ((next = reader.ReadLine ()) != null) {
				CompilerError error = CreateLinkerErrorFromErrorString (next);
				if (error != null)
					cr.Errors.Add (error);
			}
			
			reader.Close ();
		}
		
		// FIXME: needs to be improved
		private CompilerError CreateLinkerErrorFromErrorString (string errorString)
		{
			CompilerError error = new CompilerError ();
			
			error.ErrorText = errorString;
			
			return error;
		}
	}
}
