//
// Authors:
//	Christian Hergert  <chris@mosaix.net>
//	Ben Motmans  <ben.motmans@gmail.com>
//
// Copyright (C) 2005 Mosaix Communications, Inc.
// Copyright (c) 2007 Ben Motmans
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

namespace MonoDevelop.Database.Sql
{
	public class ParameterSchema : AbstractSchema
	{
		protected string dataType;
		protected ParameterType paramType;
		protected int position;
		
		public ParameterSchema (ISchemaProvider schemaProvider)
			: base (schemaProvider)
		{
		}
		
		public ParameterSchema (ParameterSchema param)
			: base (param)
		{
			this.dataType = param.dataType;
			this.paramType = param.paramType;
			this.position = param.position;
		}
		
		public DataTypeSchema DataType {
			get { return provider.GetDataType (dataType); }
		}
		
		public string DataTypeName {
			get { return dataType; }
			set {
				if (dataType != value) {
					dataType = value;
					OnChanged ();
				}
			}
		}
		
		public ParameterType ParameterType {
			get { return paramType; }
			set {
				if (paramType != value) {
					paramType = value;
					OnChanged();
				}
			}
		}
		
		public virtual int Position {
			get { return position; }
			set {
				if (position != value) {
					position = value;
					OnChanged();
				}
			}
		}
		
		public override object Clone ()
		{
			return new ParameterSchema (this);
		}
	}
}
