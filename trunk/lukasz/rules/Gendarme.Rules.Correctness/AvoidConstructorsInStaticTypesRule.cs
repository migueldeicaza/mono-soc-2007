//
// Gendarme.Rules.Correctness.AvoidConstructorsInStaticTypesRule
//
// Authors:
//	Lukasz Knop <lukasz.knop@gmail.com>
//
// Copyright (C) 2007 Lukasz Knop
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
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Gendarme.Framework;


namespace Gendarme.Rules.Correctness
{
	public class AvoidConstructorsInStaticTypesRule : ITypeRule
	{
		private const string MessageString = "Types with no instance fields or methods should not have public instance constructors";
		
		public MessageCollection CheckType(TypeDefinition type, Runner runner)
		{
			MessageCollection messageCollection = new MessageCollection();

			foreach (MethodDefinition method in type.Methods)
			{
				if (!method.IsStatic)
				{
					return null;
				}
			}

			foreach (FieldDefinition field in type.Fields)
			{
				if (!field.IsStatic)
				{
					return null;
				}
			}
			
			foreach (MethodDefinition ctor in type.Constructors)
			{
				if (!ctor.IsStatic && (ctor.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
				{
					Location location = new Location(type.Name, ctor.Name, 0);
					Message message = new Message(MessageString, location, MessageType.Error);
					messageCollection.Add(message);
				}
			}

			return messageCollection.Count > 0 ? messageCollection : null;
			
		}
	}
}
