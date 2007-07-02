//
// ProjectNavigationInformation.cs
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
using System.Collections.Generic;

using MonoDevelop.Projects;

namespace CBinding.Navigation
{
	public class ProjectNavigationInformation
	{
		private Project project;
		private List<Namespace> namespaces = new List<Namespace> ();
		private List<Function> functions = new List<Function> ();
		private List<Class> classes = new List<Class> ();
		private List<Structure> structures = new List<Structure> ();
		private List<Member> members = new List<Member> ();
		private List<Variable> variables = new List<Variable> ();
		
		public ProjectNavigationInformation (Project project)
		{
			this.project = project;
		}
		
		public void Clear ()
		{
			namespaces.Clear ();
			functions.Clear ();
			classes.Clear ();
			structures.Clear ();
			members.Clear ();
			variables.Clear ();
		}
		
		public Project Project {
			get { return project; }
		}
		
		public List<Namespace> Namespaces {
			get { return namespaces; }
		}
		
		public List<Function> Functions {
			get { return functions; }
		}
		
		public List<Class> Classes {
			get { return classes; }
		}
		
		public List<Structure> Structures {
			get { return structures; }
		}
		
		public List<Member> Members {
			get { return members; }
		}
		
		public List<Variable> Variables {
			get { return variables; }
		}
	}
}
