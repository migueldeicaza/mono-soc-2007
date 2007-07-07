//
// Unit Test for DetectLongMethod Rule.
//
// Authors:
//      Néstor Salceda <nestor.salceda@gmail.com>
//
//      (C) 2007 Néstor Salceda
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
using System.Reflection;

using Mono.Cecil;
using NUnit.Framework;
using Gendarme.Framework;
using Gendarme.Rules.Smells;

namespace Test.Rules.Smells {
	[TestFixture]
	public class DetectLongMethodTest {
		private IMethodRule rule;
		private AssemblyDefinition assembly;
		private MethodDefinition method;
		private TypeDefinition type;
		private MessageCollection messageCollection;

		[TestFixtureSetUp]
		public void FixtureSetUp () 
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
			type = assembly.MainModule.Types["Test.Rules.Smells.DetectLongMethodTest"];
			rule = new DetectLongMethodRule ();
			messageCollection = null;
		}

		public void LongMethod () 
		{
		}

		public void EmptyMethod () 
		{
		}

		public void ShortMethod ()
		{
		}

		private MethodDefinition GetMethodForTest (string methodName) 
		{
			foreach (MethodDefinition method in type.Methods) {
				if (method.Name == methodName)
					return method;
			}
			return null;
		}

		[Test]
		public void LongMethodTest () 
		{
			method = GetMethodForTest ("LongMethod");
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (1, messageCollection.Count);
		}

		[Test]
		public void EmptyMethodTest () 
		{
			method = GetMethodForTest ("EmptyMethod");
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNull (messageCollection);
		}

		[Test]
		public void ShortMethodTest () 
		{
			method = GetMethodForTest ("ShortMethod");
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNull (messageCollection);
		}
	}
}
