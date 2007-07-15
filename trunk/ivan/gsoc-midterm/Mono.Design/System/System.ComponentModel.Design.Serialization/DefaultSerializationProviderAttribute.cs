/* vi:set ts=4 sw=4: */
//
// System.ComponentModel.Design.Serialization.DefaultSerializationProviderAttribute
//
// Authors:		
//		Ivan N. Zlatev (contact i-nZ.net)
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

#if NET_2_0

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Mono.Design
{
	
	[AttributeUsageAttribute (AttributeTargets.Class, Inherited=false)] 
	public sealed class DefaultSerializationProviderAttribute : Attribute
	{
		
		private string _providerTypeName;
		
		public DefaultSerializationProviderAttribute (string providerTypeName)
		{
			if (providerTypeName == null)
				throw new ArgumentNullException ("providerTypeName");
			
			_providerTypeName = providerTypeName;
		}
		
		public DefaultSerializationProviderAttribute (Type providerType)
		{
			if (providerType == null)
				throw new ArgumentNullException ("providerType");

			_providerTypeName = providerType.AssemblyQualifiedName;
		}
		
		public string ProviderTypeName {
			get { return _providerTypeName; }
		}
	}
}
#endif
