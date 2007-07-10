//
// ProjectNodeBuilderExtension.cs
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
using System.IO;
using System.Threading;

using Mono.Addins;

using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Core.Gui;
using MonoDevelop.Components.Commands;

using CBinding;

namespace CBinding.Navigation
{
	public class ProjectNodeBuilderExtension : NodeBuilderExtension
	{
		public static event ClassPadEventHandler FinishedBuildingTree;
		public ClassPadEventHandler finishedBuildingTreeHandler;
		
		public override bool CanBuildNode (Type dataType)
		{
			return typeof(CProject).IsAssignableFrom (dataType);
		}
		
		public override Type CommandHandlerType {
			get { return typeof(ProjectNodeBuilderExtensionHandler); }
		}
		
		protected override void Initialize ()
		{
			finishedBuildingTreeHandler = (ClassPadEventHandler)MonoDevelop.Core.Gui.Services.DispatchService.GuiDispatch (new ClassPadEventHandler (OnFinishedBuildingTree));
			FinishedBuildingTree += finishedBuildingTreeHandler;
		}
		
		public override void Dispose ()
		{
			FinishedBuildingTree -= finishedBuildingTreeHandler;
		}
		
		public static void CreatePadTree (object o)
		{
			CProject p = o as CProject;
			if (o == null) return;
			
			try {
				TagDatabaseManager.Instance.WriteTags (p);
				TagDatabaseManager.Instance.FillProjectNavigationInformation (p);
			} catch (IOException ex) {
				IdeApp.Services.MessageService.ShowError (ex);
				return;
			}
			
			FinishedBuildingTree (new ClassPadEventArgs (p));
		}
		
		public override void BuildChildNodes (ITreeBuilder builder, object dataObject)
		{			
			CProject p = dataObject as CProject;
			
			if (p == null) return;
			
			bool nestedNamespaces = builder.Options["NestedNamespaces"];
			
			ProjectNavigationInformation info = ProjectNavigationInformationManager.Instance.Get (p);
			
			// Namespaces
			foreach (Namespace n in info.Namespaces) {
				if (nestedNamespaces) {
					if (n.Parent == null) {
						builder.AddChild (n);
					}
				} else {
					builder.AddChild (n);
				}
			}
			
			// Globals
			builder.AddChild (info.Globals);
			
			// Macro Definitions
			builder.AddChild (info.MacroDefinitions);
		}
		
		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return true;
		}
		
		private void OnFinishedBuildingTree (ClassPadEventArgs e)
		{
			ITreeBuilder builder = Context.GetTreeBuilder (e.Project);
			builder.UpdateChildren ();
		}
	}
	
	public class ProjectNodeBuilderExtensionHandler : NodeCommandHandler
	{
//		public override void ActivateItem ()
//		{
//			CProject p = CurrentNode.DataItem as CProject;
//			
//			if (p == null) return;
//			
//			Thread builderThread = new Thread (new ParameterizedThreadStart (ProjectNodeBuilderExtension.CreatePadTree));
//			builderThread.Name = "PadBuilder";
//			builderThread.IsBackground = true;
//			builderThread.Start (p);
//		}
		
		[CommandHandler (CProjectCommands.UpdateClassPad)]
		public void UpdateClassPad ()
		{
			CProject p = CurrentNode.DataItem as CProject;
			
			if (p == null) return;
			
			Thread builderThread = new Thread (new ParameterizedThreadStart (ProjectNodeBuilderExtension.CreatePadTree));
			builderThread.Name = "PadBuilder";
			builderThread.IsBackground = true;
			builderThread.Start (p);
		}
	}
}
