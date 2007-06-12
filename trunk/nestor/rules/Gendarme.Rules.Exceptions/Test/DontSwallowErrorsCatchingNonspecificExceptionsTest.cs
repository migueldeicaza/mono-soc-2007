//
// Unit Test for DontSwallowErrorsCatchingNonspecificExceptions Rule
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
using System.Reflection;
using System.IO;

using Gendarme.Framework;
using Gendarme.Rules.Exceptions;
using Mono.Cecil;
using NUnit.Framework;

namespace Test.Rules.Exceptions {
	

	[TestFixture]
	public class DontSwallowErrorsCatchingNonspecificExceptionsTest {
		
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
			rule = new DontSwallowErrorsCatchingNonspecificExceptions ();
			type = assembly.MainModule.Types ["Test.Rules.Exceptions.DontSwallowErrorsCatchingNonspecificExceptionsTest"];
			messageCollection = null;
		}
		
		private void CheckMessageType (MessageCollection messageCollection, MessageType messageType) 
		{
			IEnumerator enumerator = messageCollection.GetEnumerator ();
			if (enumerator.MoveNext ()) {
				Message message = (Message) enumerator.Current;
				Assert.AreEqual (message.Type, messageType);
			}
		}
		
		[Test]
		public void SwallowErrorsCatchingExceptionsEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingExceptionEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		
		[Test]
		public void SwallowErrorsCatchingExceptionsNoEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingExceptionNoEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		[Test]
		public void SwallowErrorsCatchingSystemExceptionEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingSystemExceptionEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		
		[Test]
		public void SwallowErrorsCatchingSystemExceptionNoEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingSystemExceptionNoEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		[Test]
		public void SwallowErrorsCatchingTypeExceptionEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingTypeExceptionEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		[Test]
		public void SwallowErrorsCatchingTypeExceptionNoEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingTypeExceptionNoEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		[Test]
		public void SwallowErrorsCatchingAllEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingAllEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		[Test]
		public void SwallowErrorsCatchingAllNoEmptyCatchBlockTest () 
		{
			method = type.Methods.GetMethod ("SwallowErrorsCatchingAllNoEmptyCatchBlock", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNotNull (messageCollection);
			Assert.AreEqual (messageCollection.Count, 1);
			CheckMessageType (messageCollection, MessageType.Error);
		}
		
		[Test]
		public void NotSwallowRethrowingExceptionTest () 
		{
			method = type.Methods.GetMethod ("NotSwallowRethrowingException", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNull (messageCollection); 
		}
		
		[Test]
		public void NotSwallowCatchingSpecificExceptionTest () 
		{
			method = type.Methods.GetMethod ("NotSwallowCatchingSpecificException", Type.EmptyTypes);
			messageCollection = rule.CheckMethod (method, new MinimalRunner ());
			Assert.IsNull (messageCollection); 
		}
		
		//Methods for make the tests
		public void SwallowErrorsCatchingExceptionEmptyCatchBlock () 
		{
			try { 
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (Exception exception) {
			}
		}
		
		public void SwallowErrorsCatchingExceptionNoEmptyCatchBlock () 
		{
			try { 
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (Exception exception) {
				Console.WriteLine (exception.Message);
				Console.WriteLine (exception);
			}
		}
		
		
		public void SwallowErrorsCatchingSystemExceptionEmptyCatchBlock () 
		{
			try {
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (SystemException exception) {
			}
		}
		
		
		
		public void SwallowErrorsCatchingSystemExceptionNoEmptyCatchBlock () 
		{
			try {
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (SystemException exception) {
				Console.WriteLine (exception.Message);
				Console.WriteLine (exception);
			}
		}
		
		public void SwallowErrorsCatchingTypeExceptionEmptyCatchBlock () 
		{
			try {
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (Exception) {
			}
		}

		public void SwallowErrorsCatchingTypeExceptionNoEmptyCatchBlock () 
		{
			try {
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (Exception) {
				Console.WriteLine ("Has happened an exception.");
			}
		}
		
		public void SwallowErrorsCatchingAllEmptyCatchBlock () 
		{
			try { 
				File.Open ("foo.txt", FileMode.Open);
			}
			catch {
			}
		}
		
		public void SwallowErrorsCatchingAllNoEmptyCatchBlock () 
		{
			try { 
				File.Open ("foo.txt", FileMode.Open);
			}
			catch {
				Console.WriteLine ("Has happened an exception.");
			}
		}
		
		public void NotSwallowRethrowingException () {
			try { 
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (Exception exception) {
				Console.WriteLine (exception.Message);
				Console.WriteLine (exception);
				throw exception;
				Console.WriteLine (exception.Message);
				Console.WriteLine (exception);
			}
		}
		
		public void NotSwallowCatchingSpecificException () 
		{
			try {
				File.Open ("foo.txt", FileMode.Open);
			}
			catch (FileNotFoundException exception) {
			}
		}
	}
}
