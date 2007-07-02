﻿//
// Authors:
//	Christian Hergert  <chris@mosaix.net>
//	Ben Motmans  <ben.motmans@gmail.com>
//
// Copyright (C) 2005 Mosaix Communications, Inc.
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
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Mono.Data.Sql
{
	public class MsSqlConnectionProvider : AbstractConnectionProvider
	{
		public MsSqlConnectionProvider (ConnectionSettings settings)
			: base (settings)
		{
		}

		public override bool Open (out string errorMessage)
		{
			SqlConnectionStringBuilder builder = null;
			try {	
				if (settings.UseConnectionString) {
					builder = new SqlConnectionStringBuilder (settings.ConnectionString);
				} else {
					builder = new SqlConnectionStringBuilder ();
					builder.InitialCatalog = settings.Database;
					builder.UserID = settings.Username;
					builder.Password = settings.Password;
					builder.DataSource = String.Concat (settings.Server, ",", settings.Port);
					builder.NetworkLibrary = "DBMSSOCN";
				}
				builder.Pooling = settings.EnablePooling;
				builder.MinPoolSize = settings.MinPoolSize;
				builder.MaxPoolSize = settings.MaxPoolSize;
				connection = new SqlConnection (builder.ToString ());
				connection.Open ();
				
				errorMessage = String.Empty;
				return true;
			} catch {
				errorMessage = String.Format ("Unable to connect. (CS={0})", builder == null ? "NULL" : builder.ToString ());
				return false;
			}
		}
	}
}
