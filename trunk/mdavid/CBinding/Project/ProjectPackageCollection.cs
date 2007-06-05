//
// ProjectPackageCollection.cs: Collection of pkg-config packages for the prject
//
// Authors:
//   Marcos David Marin Amador <MarcosMarin@gmail.com>
//
// Copyright (C) 2007 Marcos David Marin Amador
//
//
// This source code is licenced under The MIT License:
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
using System.Collections;

namespace CBinding
{
	[Serializable()]
	public class ProjectPackageCollection : CollectionBase
	{
		private CProject project;
		
		public ProjectPackageCollection ()
		{
		}
		
		internal void SetProject (CProject project)
		{
			this.project = project;
		}
		
		public string this[int index] {
			get { return (string)List[index]; }
			set { List[index] = value; }
		}
		
		public int Add (string package)
		{
			return List.Add (package);
		}
		
		public void AddRange (string[] packages)
		{
			foreach (string s in packages)
				List.Add (s);
		}
		
		public void AddRange (ProjectPackageCollection packages)
		{
			foreach (string s in packages)
				List.Add (s);
		}
		
		public bool Contains (string package)
		{
			return List.Contains (package);
		}
		
		public void CopyTo (string[] array, int index)
		{
			List.CopyTo (array, index);
		}
		
		public int IndexOf (string package)
		{
			return List.IndexOf (package);
		}
		
		public void Insert (int index, string package)
		{
			List.Insert (index, package);
		}
		
		public void Remove (string package)
		{
			List.Remove (package);
		}
		
		public new ProjectPackageEnumerator GetEnumerator ()
		{
			return new ProjectPackageEnumerator (this);
		}
		
		protected override void OnClear ()
		{
			if (project != null) {
				foreach (string package in (ArrayList)InnerList) {
					project.NotifyPackageRemovedFromProject (package);
				}
			}
		}
		
		protected override void OnInsertComplete (int index, object value)
		{
			if (project != null)
				project.NotifyPackageAddedToProject ((string)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			if (project != null)
				project.NotifyPackageRemovedFromProject ((string)value);
		}
		
		protected override void OnSet (int index, object oldValue, object newValue)
		{
			if (project != null)
				project.NotifyPackageRemovedFromProject ((string)oldValue);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
			if (project != null)
				project.NotifyPackageAddedToProject ((string)newValue);
		}
	}
	
	public class ProjectPackageEnumerator : IEnumerator
	{
		private IEnumerator enumerator;
		private IEnumerable temp;
		
		public ProjectPackageEnumerator (ProjectPackageCollection packages)
		{
			temp = (IEnumerable)packages;
			enumerator = temp.GetEnumerator ();
		}
		
		public string Current {
			get { return (string)enumerator.Current; }
		}
		
		object IEnumerator.Current {
			get { return enumerator.Current; }
		}
		
		public bool MoveNext ()
		{
			return enumerator.MoveNext ();
		}
		
		bool IEnumerator.MoveNext ()
		{
			return enumerator.MoveNext ();
		}
		
		public void Reset ()
		{
			enumerator.Reset ();
		}
		
		void IEnumerator.Reset ()
		{
			enumerator.Reset ();
		}
	}
}
