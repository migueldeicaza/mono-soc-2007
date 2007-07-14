//
// CFormattingStrategy.cs
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

using MonoDevelop.Ide.Gui;
using MonoDevelop.SourceEditor.FormattingStrategy;

namespace CBinding
{
	public class CFormattingStrategy : DefaultFormattingStrategy
	{
		protected override int SmartIndentLine (TextEditor d, int lineNumber, string indentString)
		{
			if (lineNumber == 0) return 0;
			
			string indentation = GetIndentation (d, lineNumber - 1);
			
			if (d.GetLineText (lineNumber - 1).TrimEnd ().EndsWith ("{")) {
				indentation += "\t";
				d.ReplaceLine (lineNumber, indentation);
			} else {
				return AutoIndentLine (d, lineNumber, indentString);
			}
			
			return indentation.Length;
		}
	}
}
