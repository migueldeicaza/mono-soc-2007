﻿//
// Authors:
//   Ben Motmans  <ben.motmans@gmail.com>
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
using MonoDevelop.Core.Gui;
using MonoDevelop.Database.Components;
namespace MonoDevelop.Database.Sql
{
	public class NpgsqlDbFactory : IDbFactory
	{
		private ISqlDialect dialect;
		private IConnectionProvider connectionProvider;
		
		public string Identifier {
			get { return "Npgsql"; }
		}
		
		public string Name {
			get { return "PostgreSQL database"; }
		}
		
		public ISqlDialect Dialect {
			get {
				if (dialect == null)
					dialect = new NpgsqlDialect ();
				return dialect;
			}
		}
		
		public IConnectionProvider ConnectionProvider {
			get {
				if (connectionProvider == null)
					connectionProvider = new NpgsqlConnectionProvider ();
				return connectionProvider;
			}
		}
		
		public IConnectionPool CreateConnectionPool (DatabaseConnectionContext context)
		{
			return new DefaultConnectionPool (this, ConnectionProvider, context);
		}
		
		public ISchemaProvider CreateSchemaProvider (IConnectionPool connectionPool)
		{
			return new NpgsqlSchemaProvider (connectionPool);
		}
		
		public object GetOption (string name)
		{
			switch (name) {
			case "settings.requires.server":
			case "settings.requires.port":
			case "settings.requires.username":
			case "settings.requires.password":
				return true;
			default:
				return null;
			}
		}
		
		public DatabaseConnectionSettings GetDefaultConnectionSettings ()
		{
			DatabaseConnectionSettings settings = new DatabaseConnectionSettings ();
			settings.ProviderIdentifier = Identifier;
			settings.Server = "localhost";
			settings.Port = 5432;
			settings.Username = "SYSDBA";
			settings.MaxPoolSize = 5;
			
			return settings;
		}
		
		public bool ShowOpenDatabaseDialog (out string database)
		{
			database = null;
			return false;
		}
	}
}