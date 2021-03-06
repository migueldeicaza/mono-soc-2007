//
// Measures.MethodMeasure class
//
// Authors:
//	Néstor Salceda <nestor.salceda@gmail.com>
//
// 	(C) 2007 Néstor Salceda
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
using System.Text;
using Mono.Cecil;

namespace Measures {

	public class MethodMeasure{
		MethodDefinition method;
		int totalLines;
		int parameters;

		internal MethodMeasure (MethodDefinition method) 
		{
			this.method = method;
		}

		public string Name {
			get {
				return method.Name;
			}
		}

		public string GetParameters () {
			StringBuilder stringBuilder = new StringBuilder ();
			foreach (ParameterReference parameter in method.Parameters) {
				if (stringBuilder.ToString () != String.Empty)
					stringBuilder.Append (", ");
				stringBuilder.Append (parameter.ParameterType.Name);
			}
			return stringBuilder.ToString ().Trim ();
		}

		public int TotalLines {
			get {
				return totalLines;
			}
			internal set {
				totalLines = value;
			}
		}

		public int Parameters {
			get {
				return parameters;
			}
			internal set {
				parameters = value;
			}
		}
	}
}
