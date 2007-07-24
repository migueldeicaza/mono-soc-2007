//
// CTextEditorExtension.cs
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

using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects.Gui.Completion;

using CBinding.Parser;

namespace CBinding
{
	public class CTextEditorExtension : TextEditorExtension
	{
		public override bool ExtendsEditor (Document doc, IEditableTextBuffer editor)
		{
			return (Path.GetExtension (doc.Title).ToUpper () == ".C"   ||
			        Path.GetExtension (doc.Title).ToUpper () == ".CPP" ||
			        Path.GetExtension (doc.Title).ToUpper () == ".CXX" ||
			        Path.GetExtension (doc.Title).ToUpper () == ".H"   ||
			        Path.GetExtension (doc.Title).ToUpper () == ".HPP");
		}
		
		public override bool KeyPress (Gdk.Key key, Gdk.ModifierType modifier)
		{
			int line, column;
			Editor.GetLineColumnFromPosition (Editor.CursorPosition, out line, out column);
			string lineText = Editor.GetLineText (line);
			
			// Formatting Strategy
			if (key == Gdk.Key.Return) {
				if (lineText.TrimEnd ().EndsWith ("{")) {
					Editor.InsertText (Editor.CursorPosition, "\n\t" + GetIndent (Editor, line));
					return false;
				}
			} else if (key == Gdk.Key.braceright && AllWhiteSpace (lineText)) {
				if (lineText.Length > 0)
					lineText = lineText.Substring (1);
				Editor.ReplaceLine (line, lineText + "}");
				return false;
			}
			
			return base.KeyPress (key, modifier);
		}
		
		public override  IParameterDataProvider HandleParameterCompletion (
		    ICodeCompletionContext completionContext, char completionChar)
		{
			if (completionChar != '(')
				return null;
			
			CProject project = Document.Project as CProject;
			
			if (project == null)
				return null;
			
			ProjectInformation info = ProjectInformationManager.Instance.Get (project);
			
			int line, column;
			Editor.GetLineColumnFromPosition (Editor.CursorPosition, out line, out column);
			int position = Editor.GetPositionFromLineColumn (line, 1);
			string lineText = Editor.GetText (position, Editor.CursorPosition - 1).TrimEnd ();
			
			int nameStart = lineText.LastIndexOfAny (new char[] { '.', ':', ' ', '\t' });

			nameStart++;
			
			string functionName = lineText.Substring (nameStart).Trim ();
			
			if (string.IsNullOrEmpty (functionName))
				return null;
			
			return new ParameterDataProvider (Editor, info, functionName);
		}
		
		private bool AllWhiteSpace (string lineText)
		{
			foreach (char c in lineText)
				if (!char.IsWhiteSpace (c))
					return false;
			
			return true;
		}
		
		// Snatched from DefaultFormattingStrategy
		private string GetIndent (TextEditor d, int lineNumber)
		{
			string lineText = d.GetLineText (lineNumber);
			StringBuilder whitespaces = new StringBuilder ();
			
			foreach (char ch in lineText) {
				if (!char.IsWhiteSpace (ch))
					break;
				whitespaces.Append (ch);
			}
			
			return whitespaces.ToString ();
		}
	}
}