// 
// Gendarme.Rules.Performance.UsingStringLengthInsteadOfCheckingEmptyStringRule
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

namespace Gendarme.Rules.Performance
{
	public class UsingStringLengthInsteadOfCheckingEmptyStringRule: IMethodRule
	{		
		public MessageCollection CheckMethod (MethodDefinition method, Runner runner)
		{
			MessageCollection messageCollection = new MessageCollection ();
			
			foreach (Instruction instruction in method.Body.Instructions) {					
				if (instruction.Operand != null) {
					if (instruction.Operand.ToString () == "System.Boolean System.String::Equals(System.String)") {
						Instruction prevInstr = instruction.Previous;
						if (prevInstr.OpCode.Name == "ldstr" && prevInstr.Operand.ToString().Length == 0) {
							Location location = new Location (method.DeclaringType.ToString(), method.Name, instruction.Offset);
							Message message = new Message ("Method uses .Equals (\"\") method to check for empty string instead of using Length property ", location, MessageType.Error);
							messageCollection.Add (message);
						}
					}
				}
			}
			
			if (messageCollection.Count == 0)
				return null;
			return messageCollection;
		}
	}
}
