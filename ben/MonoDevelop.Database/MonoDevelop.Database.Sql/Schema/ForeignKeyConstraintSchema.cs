//
// Schema/ForeignKeyConstraintSchema.cs
//
// Authors:
//   Christian Hergert	<chris@mosaix.net>
//
// Copyright (C) 2005 Mosaix Communications, Inc.
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
	public class ForeignKeyConstraintSchema : ConstraintSchema
	{
		protected string referenceTable;
		protected ColumnSchemaCollection referenceColumns;
		
		public ForeignKeyConstraintSchema (ISchemaProvider schemaProvider)
			: base (schemaProvider, ConstraintType.ForeignKey)
		{
			referenceColumns = new ColumnSchemaCollection ();
		}
		
		public TableSchema ReferenceTable {
			get { throw new NotImplementedException(); }
			set {
				referenceTable = value.FullName;
				OnChanged();
			}
		}
		
		public string ReferenceTableName {
			get { return referenceTable; }
			set { referenceTable = value; }
		}
		
		public ColumnSchemaCollection ReferenceColumns {
			get { return referenceColumns; }
		}
	}
}
