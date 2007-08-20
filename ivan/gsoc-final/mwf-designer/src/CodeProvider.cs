//
// Authors:	 
//	  Ivan N. Zlatev (contact i-nZ.net)
//
// (C) 2007 Ivan N. Zlatev

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
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Parser;
using ICSharpCode.NRefactory.Parser.CSharp;
using ICSharpCode.NRefactory.Visitors;

namespace mwf_designer
{
	internal class CodeProvider
	{
		private string _codeBehindFileName;
		private string _fileName;
		private CodeDomProvider _provider;

		// TODO: parse both and try to find the basetype of the partial codebehind
		//
		public CodeProvider (string fileName, string codeBehindFileName)
		{
			if (codeBehindFileName == null)
				throw new ArgumentNullException ("codeBehindFileName");
			if (fileName == null)
				throw new ArgumentNullException ("fileName");

			_codeBehindFileName = codeBehindFileName;
			_fileName = fileName;
			_provider = GetCodeProvider (fileName);
		}

		public CodeDomProvider CodeDomProvider {
			get { return _provider; }
			set { _provider = value; }
		}

		public CodeCompileUnit Parse ()
		{	
			return MergeParse (_fileName, _codeBehindFileName);
		}

		// The .Designer partial file produced by Visual Studio and SharpDevelop
		// does not specify the inherited type. The type is specified in the 
		// non-designer generated file.E.g:
		// 
		// partial Form1 - in Form1.Designer.cs
		// partial Form1 : Form - in Form1.cs
		//
		private CodeCompileUnit MergeParse (string fileName, string codeBehindFileName)
		{
			CodeCompileUnit mergedUnit = null;

			// parse codebehind
			IParser codeBehindParser =  ICSharpCode.NRefactory.ParserFactory.CreateParser (codeBehindFileName);
			codeBehindParser.Parse ();

			// get the first valid typedeclaration name
			CodeDomVisitor visitor = new CodeDomVisitor ();
			visitor.VisitCompilationUnit (codeBehindParser.CompilationUnit, null);
			mergedUnit = visitor.codeCompileUnit;

			string codeBehindNamespaceName = null;
			CodeTypeDeclaration codeBehindType = GetFirstValidType (visitor.codeCompileUnit, out codeBehindNamespaceName);
			if (codeBehindType == null)
				throw new InvalidOperationException ("No class with an InitializeComponent method found");

			// parse file without the method bodies
			IParser fileParser = ICSharpCode.NRefactory.ParserFactory.CreateParser (fileName);
			fileParser.ParseMethodBodies = false;
			fileParser.Parse ();

			// match declaration name from codebehind and get the type
			visitor = new CodeDomVisitor ();
			visitor.VisitCompilationUnit (fileParser.CompilationUnit, null);
			foreach (CodeNamespace namesp in visitor.codeCompileUnit.Namespaces) {
				if (namesp.Name == codeBehindNamespaceName) {
					foreach (CodeTypeDeclaration declaration in namesp.Types) {
						if (declaration.Name == codeBehindType.Name) {
							foreach (CodeTypeReference baseType in declaration.BaseTypes)
								codeBehindType.BaseTypes.Add (baseType);
						}
					}
				}
			}

			fileParser.Dispose ();
			codeBehindParser.Dispose ();
			return mergedUnit;
		}

		// Gets the first valid Class with an InitializeComponent method
		//
		private CodeTypeDeclaration GetFirstValidType (CodeCompileUnit document, out string namespaceName)
		{
			namespaceName = null;

			foreach (CodeNamespace namesp in document.Namespaces) {
				foreach (CodeTypeDeclaration declaration in namesp.Types) {
					if (declaration.IsClass) {
						foreach (CodeTypeMember member in declaration.Members) {
							CodeMemberMethod method = member as CodeMemberMethod;
							if (method != null && method.Name == "InitializeComponent") {
								namespaceName = namesp.Name;
								return declaration;
							}
						}
					}
				}
			}
			return null;
		}

		public void Write (CodeCompileUnit unit)
		{
			PreProcessCompileUnit (unit);

			string namesp = null;
			CodeTypeDeclaration codeBehindType = GetFirstValidType (unit, out namesp);
			codeBehindType.BaseTypes.Clear (); // to match VS/SD

			StreamWriter writer = new StreamWriter (_codeBehindFileName, false /* append */);

			CodeGeneratorOptions options = new CodeGeneratorOptions ();
			options.BracingStyle = "C";
			options.BlankLinesBetweenMembers = false;
			options.VerbatimOrder = true;
			_provider.GenerateCodeFromCompileUnit (unit, writer, options);

			writer.Close();
			writer.Dispose ();
		}

		private void PreProcessCompileUnit (CodeCompileUnit unit)
		{

			// 1. remove the "Global" namespace
			//
			int index = -1;
			for (int i=0; i < unit.Namespaces.Count; i++) {
				if (unit.Namespaces[i].Name == "Global") {
					index = i;
					break;
				}
			}
			unit.Namespaces.RemoveAt (index);

			// 2. Make class as partial and public
			//
			string namesp = null;
			CodeTypeDeclaration type = GetFirstValidType (unit, out namesp);
			type.IsPartial = true;
			type.TypeAttributes = TypeAttributes.Public;

			// 3. clear using statements
			//
			CodeNamespace nspace = GetNamespace (namesp, unit);
			nspace.Imports.Clear ();
		}

		private CodeNamespace GetNamespace (string name, CodeCompileUnit unit)
		{
			foreach (CodeNamespace n in unit.Namespaces)
				if (n.Name == name)
					return n;
			return null;
		}

		private CodeDomProvider GetCodeProvider (string fileName)
		{
			CodeDomProvider provider = null;
			string extension = Path.GetExtension (fileName);

			if (String.Compare (extension, ".cs", true) == 0) {
				provider = new Microsoft.CSharp.CSharpCodeProvider ();
			} else if (String.Compare (extension, ".vb", true) == 0) {
				provider = new Microsoft.VisualBasic.VBCodeProvider ();
			} else {
				throw new InvalidOperationException ("Programming language not supported for:" + fileName);
			}
			return provider;
		}


		public ICollection GetCompatibleMethods (ParameterInfo[] parameters)
		{
			ArrayList methodNames = new ArrayList ();
			IParser fileParser = ICSharpCode.NRefactory.ParserFactory.CreateParser (_fileName);
			fileParser.ParseMethodBodies = false;
			fileParser.Parse ();

			TypeDeclaration klass = GetFirstValidType (fileParser.CompilationUnit);
			if (klass != null) {
				foreach (INode child in fileParser.CompilationUnit.Children) {
					MethodDeclaration methodDeclaration = child as MethodDeclaration;
					if (methodDeclaration.Parameters.Count == parameters.Length) {
						bool match = false;
						for (int i=0; i < methodDeclaration.Parameters.Count; i++) {
							if (methodDeclaration.Parameters[i].TypeReference.Type != parameters[i].ParameterType.Name)
								match = true;
							else if (methodDeclaration.Parameters[i].TypeReference.SystemType == parameters[i].ParameterType.Name)
								match = true;
							if (!match)
								break;
						}
						if (match)
							methodNames.Add (methodDeclaration.Name);
					}
				}
			}

			fileParser.Dispose ();
			return methodNames;
		}

		private TypeDeclaration GetFirstValidType (CompilationUnit document)
		{
			foreach (INode child in document.Children) {
				NamespaceDeclaration namesp = child as NamespaceDeclaration;
				if (namesp != null) {
					foreach (INode child1 in namesp.Children) {
						TypeDeclaration declaration = child1 as TypeDeclaration;
						if (declaration != null) {
							foreach (INode child2 in declaration.Children) {
								MethodDeclaration methodDecl = child2 as MethodDeclaration;
								if (methodDecl.Name == "InitializeComponent")
									return declaration;
							}
						}
					}
				}
			}
			return null;
		}
	}
}
