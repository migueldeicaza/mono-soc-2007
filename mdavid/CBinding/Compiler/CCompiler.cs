//
// CCompiler.cs: asbtract class that provides some basic implementation for ICompiler
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

using System.Text;
using System.CodeDom.Compiler;

using Mono.Addins;

using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace CBinding
{
	public abstract class CCompiler : ICompiler
	{
		protected string compilerCommand;
		protected string linkerCommand;
		
		public abstract string Name {
			get;
		}
			
		public abstract Language Language {
			get;
		}
		
		public string CompilerCommand {
			get { return compilerCommand; }
		}
		
		public abstract string GetCompilerFlags (CProjectConfiguration configuration);
		
		public abstract ICompilerResult Compile (
			ProjectFileCollection projectFiles,
		    ProjectPackageCollection packages,
		    CProjectConfiguration configuration,
		    IProgressMonitor monitor);
		    
		protected abstract void ParseCompilerOutput (string errorString, CompilerResults cr);
		
		protected abstract void ParseLinkerOutput (string errorString, CompilerResults cr);
		
		protected string GeneratePkgLinkerArgs (ProjectPackageCollection packages)
		{
			if (packages == null || packages.Count < 1)
				return string.Empty;
			
			StringBuilder libs = new StringBuilder ();
			
			foreach (ProjectPackage p in packages)
				libs.Append (p.File + " ");
			
			return string.Format ("`pkg-config --libs {0}`", libs.ToString ().Trim ());
		}
		
		protected string GeneratePkgCompilerArgs (ProjectPackageCollection packages)
		{
			if (packages == null || packages.Count < 1)
				return string.Empty;
			
			StringBuilder libs = new StringBuilder ();
			
			foreach (ProjectPackage p in packages)
				libs.Append (p.File + " ");
			
			return string.Format ("`pkg-config --cflags {0}`", libs.ToString ().Trim ());
		}
	}
}
