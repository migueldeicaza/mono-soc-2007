// 
// Unit tests for UsingCloneWithoutImplementingICloneableRule
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
using System.Reflection;

using Gendarme.Framework;
using Gendarme.Rules.Design;
using Mono.Cecil;
using NUnit.Framework;

namespace Test.Rules.Design
{
	[TestFixture]
	public class UsingCloneWithoutImplementingICloneableTest {
		public class UsingCloneAndImplementingICloneable: ICloneable
		{
			public object Clone ()
			{
				return this.MemberwiseClone ();
			}
		}
		
		public class UsingCloneWithoutImplementingICloneable 
		{
			public object Clone ()
			{
				return this.MemberwiseClone ();
			}
		}
		
		public class NeitherUsingCloneNorImplementingICloneable
		{
			public object clone ()
			{
				return this;
			}
		}
		
		public class AnotherExampleOfNotUsingBoth
		{
			public int Clone ()
			{
				return 1;
			}
		}
		
		public class OneMoreExample
		{
			public object Clone (int i)
			{
				return this.MemberwiseClone ();
			}
		}
		
		private ITypeRule typeRule;
		private AssemblyDefinition assembly;
		private TypeDefinition type;
		MessageCollection messageCollection;
		
		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
			typeRule = new UsingCloneWithoutImplementingICloneableRule ();
			messageCollection = null;
		}
		
		private TypeDefinition GetTest (string name)
		{
			string fullname = "Test.Rules.Design.UsingCloneWithoutImplementingICloneableTest/" + name;
			return assembly.MainModule.Types[fullname];
		}
			
		[Test]
		public void usingCloneAndImplementingICloneableTest ()
		{
			type = GetTest ("UsingCloneAndImplementingICloneable");
			messageCollection = typeRule.CheckType (type, new MinimalRunner ());
			Assert.IsNull (messageCollection);
		}
		
		[Test]
		public void usingCloneWithoutImplementingICloneableTest ()
		{
			type = GetTest ("UsingCloneWithoutImplementingICloneable");
			messageCollection = typeRule.CheckType (type, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (1, messageCollection.Count);
		}
		
		[Test]
		public void neitherUsingCloneNorImplementingICloneableTest ()
		{
			type = GetTest ("NeitherUsingCloneNorImplementingICloneable");
			messageCollection = typeRule.CheckType (type, new MinimalRunner ());
			Assert.IsNull (messageCollection);
		}
		
		[Test]
		public void anotherExampleOfNotUsingBothTest ()
		{
			type = GetTest ("AnotherExampleOfNotUsingBoth");
			messageCollection = typeRule.CheckType (type, new MinimalRunner ());
			Assert.IsNull (messageCollection);
		}
		
		[Test]
		public void OneMoreExampleTest ()
		{
			type = GetTest ("OneMoreExample");
			messageCollection = typeRule.CheckType (type, new MinimalRunner ());
			Assert.IsNull (messageCollection);
		}
	}
}