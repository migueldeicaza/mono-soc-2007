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
using MonoDevelop.Core;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core.Gui;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Database.Sql;
using MonoDevelop.Database.Designer;
using MonoDevelop.Database.Components;

namespace MonoDevelop.Database.ConnectionManager
{
	public class ConnectionContextCollectionNodeBuilder : TypeNodeBuilder
	{
		private ITreeBuilder builder;
		
		public ConnectionContextCollectionNodeBuilder ()
			: base ()
		{
			ConnectionContextService.ConnectionContextAdded += (DatabaseConnectionContextEventHandler)Services.DispatchService.GuiDispatch (new DatabaseConnectionContextEventHandler (OnConnectionAdded));
			ConnectionContextService.ConnectionContextRemoved += (DatabaseConnectionContextEventHandler)Services.DispatchService.GuiDispatch (new DatabaseConnectionContextEventHandler (OnConnectionRemoved));
		}
		
		public override Type NodeDataType {
			get { return typeof (DatabaseConnectionContextCollection); }
		}
		
		public override Type CommandHandlerType {
			get { return typeof (ConnectionContextCollectionCommandHandler); }
		}
		
		public override string ContextMenuAddinPath {
			get { return "/SharpDevelop/Views/ConnectionManagerPad/ContextMenu/ConnectionsNode"; }
		}
		
		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			return GettextCatalog.GetString ("Database Connections");
		}
		
		public override void BuildNode (ITreeBuilder builder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
		{
			label = GettextCatalog.GetString ("Database Connections");
			icon = Context.GetIcon ("md-db-connection");
			this.builder = builder;
		}
		
		public override void BuildChildNodes (ITreeBuilder builder, object dataObject)
		{
			DatabaseConnectionContextCollection collection = (DatabaseConnectionContextCollection) dataObject;
			Runtime.LoggingService.Debug ("BuildChildNodes CONTEXT  " + collection.Count);
				
			foreach (DatabaseConnectionContext context in collection)
				builder.AddChild (context);
			builder.Expanded = true;
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			Runtime.LoggingService.Debug ("HasChildNodes CONTEXT");
			DatabaseConnectionContextCollection collection = (DatabaseConnectionContextCollection) dataObject;
			return collection.Count > 0;
		}
		
		private void OnConnectionAdded (object sender, DatabaseConnectionContextEventArgs args)
		{
			builder.AddChild (args.ConnectionContext);
		}
		
		private void OnConnectionRemoved (object sender, DatabaseConnectionContextEventArgs args)
		{
			if (builder.MoveToObject (args.ConnectionContext)) {
				builder.Remove ();
				builder.MoveToParent ();
			}
		}
	}
	
	public class ConnectionContextCollectionCommandHandler : NodeCommandHandler
	{
		public override DragOperation CanDragNode ()
		{
			return DragOperation.None;
		}

		[CommandHandler (ConnectionManagerCommands.CreateDatabase)]
		protected void OnCreateDatabase ()
		{
			CreateDatabaseDialog dlg = new CreateDatabaseDialog ();
			if (dlg.Run () == (int)ResponseType.Ok) {
				DatabaseConnectionContext context = dlg.DatabaseConnection;
				WaitDialog.ShowDialog ("Creating database ...");
				ThreadPool.QueueUserWorkItem (new WaitCallback (OnCreateDatabaseThreaded), context);
			}
			dlg.Destroy ();
		}
		
		private void OnCreateDatabaseThreaded (object state)
		{
			DatabaseConnectionContext context = state as DatabaseConnectionContext;
			
			ISchemaProvider schemaProvider = context.SchemaProvider;
			DatabaseSchema db = new DatabaseSchema (schemaProvider);
			db.Name = context.ConnectionSettings.Database;
			
			schemaProvider.CreateDatabase (db);
			
			Services.DispatchService.GuiDispatch (delegate () {
				WaitDialog.HideDialog ();
				ConnectionContextService.AddDatabaseConnectionContext (context);
			});
		}
	}
}
