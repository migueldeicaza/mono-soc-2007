//
// Gendarme.Rules.Naming.AttributeEndsWithAttributeSuffixRule class
//
// Authors:
//	Néstor Salceda <nestor.salceda@gmail.com>
//
//  (C) 2007 Néstor Salceda
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

using Mono.Cecil;
using Gendarme.Framework;

namespace Gendarme.Rules.Naming {
	public class AttributeEndsWithAttributeSuffixRule : ITypeRule {
		private enum InheritanceResult {
			InheritsFromAttribute,
			NoInheritsFromAttribute,
			Undetermined
		}
	
		private InheritanceResult InheritsFromAttribute (TypeReference typeReference)
		{
			if (typeReference is TypeDefinition) {
				TypeDefinition current = (TypeDefinition) typeReference;
				while (current.BaseType.FullName != "System.Object") {
					if (current.BaseType.FullName == "System.Attribute")
						return InheritanceResult.InheritsFromAttribute;
					else {
					    if (current.BaseType is TypeDefinition) 
					    	current = (TypeDefinition) current.BaseType;
					    else
					    	return InheritanceResult.Undetermined;
					}
				}
				return InheritanceResult.NoInheritsFromAttribute;
			}
			else 
				return InheritanceResult.Undetermined;
		}
		
		public MessageCollection CheckType (TypeDefinition typeDefinition, Runner runner)
		{
			MessageCollection messageCollection = new MessageCollection ();
			switch (InheritsFromAttribute (typeDefinition)) {
			case InheritanceResult.InheritsFromAttribute:
				if (!typeDefinition.Name.EndsWith ("Attribute")) {
					Location location = new Location (typeDefinition.FullName, typeDefinition.Name, 0);
					Message message = new Message ("The class name doesn't end with Attribute Suffix", location, MessageType.Error);
					messageCollection.Add (message);                        
				}
				break;
			case InheritanceResult.Undetermined:
				Location location = new Location (typeDefinition.FullName, typeDefinition.Name, 0);
				Message message = new Message ("I can't determine if this type inherits from Attribute.", location, MessageType.Warning);
				messageCollection.Add (message);
				break;
			case InheritanceResult.NoInheritsFromAttribute:
				break;
			default:
				break;
			}
			if (messageCollection.Count == 0)
				return null;
			return messageCollection;
		}
	}
}