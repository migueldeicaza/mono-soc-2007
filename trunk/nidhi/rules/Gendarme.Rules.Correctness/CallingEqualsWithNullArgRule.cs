// 
// Gendarme.Rules.Correctness.CallingEqualsWithNullArgRule
//
// Authors:
//	Nidhi Rawal <sonu2404@gmail.com>
//
// Copyright (c) <2007> Nidhi Rawal
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Gendarme.Framework;

namespace Gendarme.Rules.Correctness {
	public class CallingEqualsWithNullArgRule: IMethodRule
	{
		public MessageCollection CheckMethod (MethodDefinition method, Runner runner)
		{			
			MessageCollection messageCollection = new MessageCollection ();
			
			foreach (Instruction ins in method.Body.Instructions)
				if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) {
					MethodReference calledMethod = (MethodReference) ins.Operand;
					if (calledMethod.Name == "Equals" && calledMethod.ReturnType.ReturnType.FullName == "System.Boolean")
						if (ins.Previous.OpCode == OpCodes.Ldnull) {
							Location location = new Location (method.DeclaringType.FullName, method.Name, 0);
							Message message = new Message ("You should not call Equals (null), i.e., argument should not be null", location, MessageType.Error);
							messageCollection.Add (message);
						}
				}
		
			if (messageCollection.Count == 0)
				return null;
			return messageCollection;
		}
	}
}