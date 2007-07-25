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

namespace MonoDevelop.Database.Sql
{
	[AttributeUsage (AttributeTargets.Class)]
	public sealed class ConnectionSettingsMetaDataAttribute : Attribute
	{
		private bool requiresUsername;
		private bool requiresPassword;
		private bool requiresServer;
		private bool requiresPort;
		
		private bool canListDatabases;
		private bool canSelectDatabase;
		
		public ConnectionSettingsMetaDataAttribute (bool requiresServer, bool requiresPort, bool requiresUsername,
			bool requiresPassword, bool canListDatabases, bool canSelectDatabase)
		{
			this.requiresServer = requiresServer;
			this.requiresPort = requiresPort;
			this.requiresUsername = requiresUsername;
			this.requiresPort = requiresPort;
			this.canListDatabases = canListDatabases;
			this.canSelectDatabase = canSelectDatabase;
		}
		
		public bool RequiresUsername {
			get { return requiresUsername; }
		}

		public bool RequiresPassword {
			get { return requiresPassword; }
		}

		public bool RequiresServer {
			get { return requiresServer; }
		}

		public bool RequiresPort {
			get { return requiresPort; }
		}
		
		public bool CanListDatabases {
			get { return canListDatabases; }
		}

		public bool CanSelectDatabase {
			get { return canSelectDatabase; }
		}
	}
}
