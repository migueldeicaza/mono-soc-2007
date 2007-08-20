/* vi:set ts=4 sw=4: */
//
// System.ComponentModel.Design.Serialization.CodeDomComponentSerializationService
//
// Authors:	 
//	  Ivan N. Zlatev (contact@i-nZ.net)
//
// (C) 2007 Ivan N. Zlatev

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

// STUBS ONLY

#if NET_2_0

using System;
using System.ComponentModel;
using System.Collections;
using System.IO;

namespace Mono.Design 
{
	public sealed class CodeDomComponentSerializationService : ComponentSerializationService
	{

		public CodeDomComponentSerializationService () : this (null)
		{
		}

		public CodeDomComponentSerializationService (IServiceProvider provider)
		{
		}

		public override SerializationStore CreateStore ()
		{
			throw new NotImplementedException ();
		}

		public override ICollection Deserialize (SerializationStore store)
		{
			throw new NotImplementedException ();
		}

		public override ICollection Deserialize (SerializationStore store, IContainer container)
		{
			throw new NotImplementedException ();
		}

		public override SerializationStore LoadStore (Stream stream)
		{
			throw new NotImplementedException ();
		}

		public override void Serialize (SerializationStore store, object value)
		{
			throw new NotImplementedException ();
		}

		public override void SerializeAbsolute (SerializationStore store, object value)
		{
			throw new NotImplementedException ();
		}

		public override void SerializeMember (SerializationStore store, object owningObject, MemberDescriptor member)
		{
			throw new NotImplementedException ();
		}

		public override void SerializeMemberAbsolute (SerializationStore store, object owningObject, MemberDescriptor member)
		{
			throw new NotImplementedException ();
		}

		public override void DeserializeTo (SerializationStore store, IContainer container, bool validateRecycledTypes, bool applyDefaults)
		{
			throw new NotImplementedException ();
		}
	}
}
#endif
