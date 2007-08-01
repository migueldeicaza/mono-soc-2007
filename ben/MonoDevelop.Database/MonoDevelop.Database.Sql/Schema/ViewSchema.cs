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
	public class ViewSchema : AbstractSchema
	{
		protected bool isSystemView = false;
		protected string statement;
		protected ColumnSchemaCollection columns;
		
		public ViewSchema (ISchemaProvider schemaProvider)
			: base (schemaProvider)
		{
		}
		
		public ViewSchema (ViewSchema view)
			: base (view)
		{
			this.isSystemView = view.isSystemView;
			this.statement = view.statement;
			this.columns = new ColumnSchemaCollection (view.columns);
		}

		public bool IsSystemView {
			get { return isSystemView; }
			set {
				if (isSystemView != value) {
					isSystemView = value;
					OnChanged();
				}
			}
		}
		
		public string Statement {
			get { return statement; }
			set {
				if (statement != value) {
					statement = value;
					OnChanged();
				}
			}
		}
		
		/// <summary>
		/// Collection of columns associated with this view.
		/// </summary>
		public ColumnSchemaCollection Columns {
			get {
				if (columns == null)
					columns = provider.GetViewColumns(this);
				return columns;
			}
		}
		
		/// <summary>
		/// Refresh the information associated with this view.
		/// </summary>
		public override void Refresh()
		{
			definition = null;
		}
		
		public override object Clone ()
		{
			ViewSchema clone = new ViewSchema (this);
			if (clone.columns != null) {
				foreach (ColumnSchema column in clone.columns)
					column.Parent = clone;
			}
			return clone;
		}
	}
}
