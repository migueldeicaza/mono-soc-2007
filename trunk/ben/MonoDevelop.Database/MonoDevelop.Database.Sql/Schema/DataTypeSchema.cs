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
using System.Collections.Generic;

namespace MonoDevelop.Database.Sql
{
	public class DataTypeSchema : AbstractSchema
	{
		protected bool isComplex;
		protected bool isNullable;
		protected bool isAutoincrementable;
		protected bool isFixedLength;
		
		protected DataTypeCategory category;
		protected Type dataType;
		
		protected string createFormat;
		protected string createParameters;

		protected Range lengthRange;
		protected Range precisionRange;
		protected Range scaleRange;
		
		public DataTypeSchema (ISchemaProvider schemaProvider)
			: base (schemaProvider)
		{
		}
		
		public DataTypeCategory DataTypeCategory {
			get { return category; }
			set {
				if (category != value) {
					category = value;
					OnChanged ();
				}
			}
		}

		public bool HasRange {
			get { return precisionRange != null; }
		}
		
		public bool HasScale {
			get { return scaleRange != null; }
		}

		public Range LengthRange {
			get { return lengthRange; }
			set {
				if (lengthRange != value) {
					lengthRange = value;
					OnChanged ();
				}
			}
		}
		
		public Range PrecisionRange {
			get { return precisionRange; }
			set {
				if (precisionRange != value) {
					precisionRange = value;
					OnChanged ();
				}
			}
		}
		
		public Range ScaleRange {
			get { return scaleRange; }
			set {
				if (scaleRange != value) {
					scaleRange = value;
					OnChanged ();
				}
			}
		}
		
		public bool IsComplex {
			get { return isComplex; }
			set {
				if (isComplex != value) {
					isComplex = value;
					OnChanged();
				}
			}
		}
		
		public bool IsNullable {
			get { return isNullable; }
			set {
				if (isNullable != value) {
					isNullable = value;
					OnChanged();
				}
			}
		}
		
		public bool IsAutoincrementable {
			get { return isAutoincrementable; }
			set {
				if (isAutoincrementable != value) {
					isAutoincrementable = value;
					OnChanged();
				}
			}
		}
		
		public bool IsFixedLength {
			get { return isFixedLength; }
			set {
				if (isFixedLength != value) {
					isFixedLength = value;
					OnChanged();
				}
			}
		}
		
		public Type DataType {
			get { return dataType; }
			set {
				if (dataType != value) {
					dataType = value;
					OnChanged();
				}
			}
		}
		
		public string CreateFormat {
			get { return createFormat; }
			set {
				if (createFormat != value) {
					createFormat = value;
					OnChanged();
				}
			}
		}
		
		public string CreateParameters {
			get { return createParameters; }
			set {
				if (createParameters != value) {
					createParameters = value;
					OnChanged();
				}
			}
		}
		
		public ColumnSchemaCollection Columns {
			get {
				if (isComplex == false) {
					return new ColumnSchemaCollection ();
				} else {
					// TODO: Get complex columns from the provider
					throw new NotImplementedException();
				}
			}
		}
	}
}
