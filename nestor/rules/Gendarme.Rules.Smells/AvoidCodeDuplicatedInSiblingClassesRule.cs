//
// Gendarme.Rules.Smells.AvoidCodeDuplicatedInSiblingClassesRule class
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
using System.Collections;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Gendarme.Framework;

namespace Gendarme.Rules.Smells {
	
	public class AvoidCodeDuplicatedInSiblingClassesRule : ITypeRule {
		private MessageCollection messageCollection;
		private CodeDuplicatedLocator codeDuplicatedLocator;

		private void FindCodeDuplicated (TypeDefinition type, ICollection siblingClasses) 
		{
			foreach (MethodDefinition method in type.Methods) {
				foreach (TypeDefinition sibling in siblingClasses) {
					foreach (Message message in codeDuplicatedLocator.CompareMethodAgainstTypeMethods (method, sibling)) {
						messageCollection.Add (message);
					}
				}
			}
		}

		private void CompareSiblingClasses (ICollection siblingClasses) 
		{
			foreach (TypeDefinition type in siblingClasses) {
				FindCodeDuplicated (type, siblingClasses);
				codeDuplicatedLocator.CheckedTypes.Add (type.Name);
			}
		}

		public MessageCollection CheckType (TypeDefinition type, Runner runner) 
		{
			messageCollection = new MessageCollection ();
			codeDuplicatedLocator = new CodeDuplicatedLocator ();
			
			ICollection siblingClasses = Utilities.GetInheritedClassesFrom (type);
			if (siblingClasses.Count >= 2) 
				CompareSiblingClasses (siblingClasses);

			if (messageCollection.Count == 0)
				return null;
			return messageCollection;
		}
	}
}
