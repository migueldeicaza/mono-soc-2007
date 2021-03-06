//
// Authors:
//    Ben Motmans  <ben.motmans@gmail.com>
//
// Copyright (c) 2007 Ben Motmans
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
//

using Gtk;
using System;
using System.Data;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Database.Designer;
using MonoDevelop.Database.Components;
namespace MonoDevelop.Database.Sql
{
	public class SqliteGuiProvider : IGuiProvider
	{
		public bool ShowSelectDatabaseDialog (bool create, out string database)
		{
			FileChooserDialog dlg = null;
			if (create) {
				dlg = new FileChooserDialog (
					GettextCatalog.GetString ("Save Database"), null, FileChooserAction.Save,
					"gtk-cancel", ResponseType.Cancel,
					"gtk-save", ResponseType.Accept
				);
			} else {
				dlg = new FileChooserDialog (
					GettextCatalog.GetString ("Open Database"), null, FileChooserAction.Open,
					"gtk-cancel", ResponseType.Cancel,
					"gtk-open", ResponseType.Accept
				);
			}
			dlg.SelectMultiple = false;
			dlg.LocalOnly = true;
			dlg.Modal = true;
			
			FileFilter filter = new FileFilter ();
			filter.AddMimeType ("application/x-sqlite2");
			filter.AddMimeType ("application/x-sqlite3");
			filter.AddPattern ("*.db");
			filter.Name = GettextCatalog.GetString ("SQLite databases");
			FileFilter filterAll = new FileFilter ();
			filterAll.AddPattern ("*");
			filterAll.Name = GettextCatalog.GetString ("All files");
			dlg.AddFilter (filter);
			dlg.AddFilter (filterAll);

			if (dlg.Run () == (int)ResponseType.Accept) {
				database = dlg.Filename;
				dlg.Destroy ();
				return true;
			} else {
				dlg.Destroy ();
				database = null;
				return false;
			}
		}

		public bool ShowTableEditorDialog (ISchemaProvider schemaProvider, TableSchema table, bool create)
		{
			return RunDialog (new TableEditorDialog (schemaProvider, table, create));
		}

		public bool ShowViewEditorDialog (ISchemaProvider schemaProvider, ViewSchema view, bool create)
		{
			return RunDialog (new ViewEditorDialog (schemaProvider, view, create));
		}

		public bool ShowProcedureEditorDialog (ISchemaProvider schemaProvider, ProcedureSchema procedure, bool create)
		{
			return RunDialog (new ProcedureEditorDialog (schemaProvider, procedure, create));
		}

		public bool ShowUserEditorDialog (ISchemaProvider schemaProvider, UserSchema user, bool create)
		{
			return RunDialog (new UserEditorDialog (schemaProvider, user, create));
		}

		private bool RunDialog (Dialog dlg)
		{
			bool result = false;
			try {	
				if (dlg.Run () == (int)ResponseType.Ok)
					result = true;
			} finally {
				dlg.Destroy ();
			}
			return result;
		}
	}
}