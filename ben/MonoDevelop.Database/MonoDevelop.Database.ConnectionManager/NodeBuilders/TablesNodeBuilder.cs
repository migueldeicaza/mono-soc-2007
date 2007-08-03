//
// Authors:
//   Christian Hergert	<chris@mosaix.net>
//   Ben Motmans  <ben.motmans@gmail.com>
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

using Gtk;
using System;
using System.Threading;
using System.Collections.Generic;
using MonoDevelop.Database.Sql;
using MonoDevelop.Database.Components;
using MonoDevelop.Database.Designer;
using MonoDevelop.Core;
using MonoDevelop.Core.Gui;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Database.ConnectionManager
{
	public class TablesNodeBuilder : TypeNodeBuilder
	{
		private EventHandler RefreshHandler;
		
		public TablesNodeBuilder ()
			: base ()
		{
			RefreshHandler = new EventHandler (OnRefreshEvent);
		}
		
		public override Type NodeDataType {
			get { return typeof (TablesNode); }
		}
		
		public override string ContextMenuAddinPath {
			get { return "/SharpDevelop/Views/ConnectionManagerPad/ContextMenu/TablesNode"; }
		}
		
		public override Type CommandHandlerType {
			get { return typeof (TablesNodeCommandHandler); }
		}
		
		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			return GettextCatalog.GetString ("Tables");
		}
		
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
		{
			label = GettextCatalog.GetString ("Tables");
			icon = Context.GetIcon ("md-db-tables");
			
			BaseNode node = (BaseNode) dataObject;
			node.RefreshEvent += RefreshHandler;
		}
		
		public override void BuildChildNodes (ITreeBuilder builder, object dataObject)
		{
			ThreadPool.QueueUserWorkItem (new WaitCallback (BuildChildNodesThreaded), dataObject);
		}
		
		private void BuildChildNodesThreaded (object state)
		{
			BaseNode node = state as BaseNode;
			ITreeBuilder builder = Context.GetTreeBuilder (state);
			bool showSystemObjects = (bool)builder.Options["ShowSystemObjects"];
			TableSchemaCollection tables = node.ConnectionContext.SchemaProvider.GetTables ();
			
			foreach (TableSchema table in tables) {
				if (table.IsSystemTable && !showSystemObjects)
					continue;
				
				DispatchService.GuiDispatch (delegate {
					builder.AddChild (new TableNode (node.ConnectionContext, table));
					builder.Expanded = true;
				});
			}
		}
		
		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return true;
		}
		
		private void OnRefreshEvent (object sender, EventArgs args)
		{
			ITreeBuilder builder = Context.GetTreeBuilder ();
			
			if (builder != null)
				builder.UpdateChildren ();
			
			builder.ExpandToNode ();
		}
	}
	
	public class TablesNodeCommandHandler : NodeCommandHandler
	{
		public override DragOperation CanDragNode ()
		{
			return DragOperation.None;
		}
		
		[CommandHandler (ConnectionManagerCommands.Refresh)]
		protected void OnRefresh ()
		{
			BaseNode node = CurrentNode.DataItem as BaseNode;
			node.Refresh ();
		}
		
		[CommandHandler (ConnectionManagerCommands.CreateTable)]
		protected void OnCreateTable ()
		{
			BaseNode node = CurrentNode.DataItem as BaseNode;
			IDbFactory fac = node.ConnectionContext.DbFactory;
			ISchemaProvider schemaProvider = node.ConnectionContext.SchemaProvider;
			TableSchema table = schemaProvider.GetNewTableSchema ("NewTable");

			if (fac.GuiProvider.ShowTableEditorDialog (schemaProvider, table, true))
				ThreadPool.QueueUserWorkItem (new WaitCallback (OnCreateTableThreaded), new object[] {schemaProvider, table, node} as object);
		}
		
		private void OnCreateTableThreaded (object state)
		{
			object[] objs = state as object[];
			
			ISchemaProvider provider = objs[0] as ISchemaProvider;
			TableSchema table = objs[1] as TableSchema;
			BaseNode node = objs[2] as BaseNode;
			
			string sql = table.Definition;
			
			IPooledDbConnection conn = provider.ConnectionPool.Request ();
			conn.ExecuteNonQuery (sql);
			conn.Release ();
			
			node.Refresh ();
		}
		
		[CommandUpdateHandler (ConnectionManagerCommands.CreateTable)]
		protected void OnUpdateCreateTable (CommandInfo info)
		{
			BaseNode node = (BaseNode)CurrentNode.DataItem;
			info.Enabled = MetaDataService.IsTableMetaDataSupported (node.ConnectionContext.SchemaProvider, TableMetaData.Create);
		}
	}
}
