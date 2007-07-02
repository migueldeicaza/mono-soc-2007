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

namespace Mono.Data.Sql
{
	public class ProcedureSchema : AbstractSchema
	{
		protected ICollection<ParameterSchema> parameters;
		protected ICollection<ColumnSchema> columns;
		protected string language = String.Empty;
		protected bool isSystemProcedure = false;
		
		public ProcedureSchema (ISchemaProvider schemaProvider)
			: base (schemaProvider)
		{
		}
		
		public ICollection<ParameterSchema> Parameters {
			get {
				if (parameters == null)
					parameters = provider.GetProcedureParameters (this);
				return parameters;
			}
		}
		
		public ICollection<ColumnSchema> Columns {
			get {
				if (columns == null)
					columns = provider.GetProcedureColumns (this);
				return columns;
			}
		}
		
		public LanguageSchema Language {
			get {
				throw new NotImplementedException();
			}
			set {
				language = value.FullName;
			}
		}
		
		public string LanguageName {
			get {
				return language;
			}
			set {
				if (language != value) {
					language = value;
					OnChanged ();
				}
			}
		}
		
		public bool IsSystemProcedure {
			get {
				return isSystemProcedure;
			}
			set {
				if (isSystemProcedure != value) {
					isSystemProcedure = value;
					OnChanged ();
				}
			}
		}
	}
}