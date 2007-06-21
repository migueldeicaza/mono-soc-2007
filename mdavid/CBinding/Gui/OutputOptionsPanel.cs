//
// OutputOptionsPanel.cs: configure output options
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

using Mono.Addins;

using MonoDevelop.Core.Properties;
using MonoDevelop.Core.Gui.Dialogs;

namespace CBinding
{
	public partial class OutputOptionsPanel : Gtk.Bin
	{
		private CProjectConfiguration configuration;
		
		public OutputOptionsPanel (IProperties customizationObject)
		{
			this.Build ();
			
			table1.RowSpacing = 3;
			
			configuration = (CProjectConfiguration)customizationObject.GetProperty ("Config");
			
			outputNameTextEntry.Text = configuration.Output;
			outputPathTextEntry.Text = configuration.OutputDirectory;
			parametersTextEntry.Text = configuration.CommandLineParameters;
			
			externalConsoleCheckbox.Active = configuration.ExternalConsole;
			pauseCheckbox.Active = configuration.PauseConsoleOutput;
			
			browseButton.Clicked += OnBrowseButtonClick;
		}
		
		private void OnBrowseButtonClick (object sender, EventArgs e)
		{
			AddPathDialog dialog = new AddPathDialog (configuration.OutputDirectory);
			dialog.Run ();
			outputPathTextEntry.Text = dialog.SelectedPath;
		}
		
		public bool Store ()
		{
			if (configuration == null)
				return false;
			
			if (outputNameTextEntry != null && outputNameTextEntry.Text.Length > 0)
				configuration.Output = outputNameTextEntry.Text.Trim ();
			
			if (outputPathTextEntry.Text != null && outputPathTextEntry.Text.Length > 0)
				configuration.OutputDirectory = outputPathTextEntry.Text.Trim ();
			
			if (parametersTextEntry.Text != null && parametersTextEntry.Text.Length > 0)
				configuration.CommandLineParameters = parametersTextEntry.Text.Trim ();
			
			configuration.ExternalConsole = externalConsoleCheckbox.Active;
			configuration.PauseConsoleOutput = pauseCheckbox.Active;
			
			return true;
		}
	}
	
	public class OutputOptionsPanelBinding : AbstractOptionPanel
	{
		private OutputOptionsPanel panel;
		
		public override void LoadPanelContents ()
		{
			panel = new OutputOptionsPanel ((IProperties)CustomizationObject);
			Add (panel);
		}
		
		public override bool StorePanelContents ()
		{
			return panel.Store ();
		}
	}
}
