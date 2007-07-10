//
// Authors:
//	Ben Motmans  <ben.motmans@gmail.com>
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

using System;
using System.Collections.Generic;

namespace Mono.Data.Sql
{
	[Serializable]
	public class ConnectionSettings
	{
		private string name;
		private string providerIdentifier;
		
		private string server;
		private int port;
		private string database;
		private string username;
		private string password;
		private bool savePassword;
		
		private bool enablePooling;
		private int minPoolSize;
		private int maxPoolSize;
		
		private string connectionString;
		private bool useConnectionString;
		
		[NonSerialized]
		private IConnectionProvider connProvider;
		[NonSerialized]
		private ISchemaProvider schemaProvider;
		
		public string Name {
			get { return name; }
			set { name = value; }
		}
		
		public string ProviderIdentifier {
			get { return providerIdentifier; }
			set { providerIdentifier = value; }
		}
		
		public string Database {
			get { return database; }
			set { database = value; }
		}
		
		public string Server {
			get { return server; }
			set { server = value; }
		}
		
		public int Port {
			get { return port; }
			set { port = value; }
		}
		
		public string Username {
			get { return username; }
			set { username = value; }
		}
		
		public string Password {
			get { return password; }
			set { password = value; }
		}
		
		public bool SavePassword {
			get { return savePassword; }
			set { savePassword = value; }
		}
		
		public bool EnablePooling {
			get { return enablePooling; }
			set { enablePooling = value; }
		}
		
		public int MinPoolSize {
			get { return minPoolSize; }
			set { minPoolSize = value; }
		}
		
		public int MaxPoolSize {
			get { return maxPoolSize; }
			set { maxPoolSize = value; }
		}

		public string ConnectionString {
			get { return connectionString; }
			set { connectionString = value; }
		}
		
		public bool UseConnectionString {
			get { return useConnectionString; }
			set { useConnectionString = value; }
		}

		public IConnectionProvider ConnectionProvider {
			get {
				if (connProvider == null)
					connProvider = DbFactoryService.CreateConnectionProvider (this);
				return connProvider;
			}
		}
		
		public bool HasConnectionProvider {
			get { return connProvider != null; }
		}

		public ISchemaProvider SchemaProvider {
			get {
				if (ConnectionProvider != null) {
					if (schemaProvider == null)
						schemaProvider = DbFactoryService.CreateSchemaProvider (this, connProvider);
					return schemaProvider;
				}
				return null;
			}
		}
		
		public bool HasSchemaProvider {
			get { return schemaProvider != null; }
		}
	}
}
