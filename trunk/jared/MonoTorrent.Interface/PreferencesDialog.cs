//
// PreferencesDialog.cs
//
// Author:
//   Jared Hendry (buchan@gmail.com)
//
// Copyright (C) 2007 Jared Hendry
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
using MonoTorrent.Client;
using Gtk;
using Gnome;

namespace MonoTorrent.Interface
{
	public partial class PreferencesDialog : Gtk.Dialog
	{

	//	private UserTorrentSettings userTorrentSettings;
		private UserEngineSettings userEngineSettings;
		private PreferencesSettings prefSettings;
		
		private FileChooserButton downloadLocationButton;
		private FileChooserButton torrentStorageLocationButton;
		private FileChooserButton importLocationButton;
		
		private ListStore filterListStore;
		private ArrayList labels;
		private LabelTreeView labelTreeView;
		
		private Egg.TrayIcon trayIcon;
		private IconEntry selectIcon;
		
		private MainWindow mainWindow;
		
		public PreferencesDialog(MainWindow mainWindow)
		{
		//	this.userTorrentSettings = mainWindow.userTorrentSettings;
			this.userEngineSettings = mainWindow.userEngineSettings;
			this.prefSettings = mainWindow.prefSettings; 
			this.labels = mainWindow.labels;	
			this.filterListStore = mainWindow.labelListStore;
			this.trayIcon = mainWindow.TrayIcon;
			this.mainWindow = mainWindow;
			
			Build();
			buildFoldersPanel();
			buildImportPanel();
			buildLabelPage();
			buildConnectionPage();
			restorePreferences();
			BuildGeneralPage();
			
			upnpCheckBox.Toggled += OnUpnpCheckBoxToggled;
		}
		
		public void SetPageIndex(int index)
		{
			prefNotebook.Page = index;
		}
		
		private void BuildGeneralPage()
		{
			minimizeTrayCheckButton.Active = prefSettings.QuitOnClose;
			enableTrayCheckButton.Active = prefSettings.EnableTray;
			enableNotificationsCheckButton.Active = prefSettings.EnableNotifications;
			
			enableNotificationsCheckButton.Toggled += OnEnableNotificationsToggled;
			minimizeTrayCheckButton.Toggled += OnMinimizeTrayToggled;
			enableTrayCheckButton.Toggled += OnEnableTrayToggled;
			
			enableNotificationsCheckButton.Sensitive = prefSettings.EnableTray;
			minimizeTrayCheckButton.Sensitive = prefSettings.EnableTray;
		}
		
		private void OnEnableNotificationsToggled (object sender, EventArgs args)
		{
			prefSettings.EnableNotifications = enableNotificationsCheckButton.Active;
		}
		
		private void OnMinimizeTrayToggled (object sender, EventArgs args)
		{
			prefSettings.QuitOnClose = minimizeTrayCheckButton.Active;
		}
		
		private void OnEnableTrayToggled (object sender, EventArgs args)
		{
			prefSettings.EnableTray = enableTrayCheckButton.Active;
			if(!prefSettings.EnableTray){
				trayIcon.HideAll();
				prefSettings.QuitOnClose = prefSettings.EnableTray;
				minimizeTrayCheckButton.Active = prefSettings.EnableTray;
				enableNotificationsCheckButton.Active = prefSettings.EnableTray;
				prefSettings.EnableNotifications = prefSettings.EnableTray;
			} else {
				trayIcon.ShowAll();
			}
			
			enableNotificationsCheckButton.Sensitive = prefSettings.EnableTray;
			minimizeTrayCheckButton.Sensitive = prefSettings.EnableTray;
		}
		
		private void buildConnectionPage()
		{
			portSpinButton.SetRange(0, 65535);
			maxConnectionsSpinButton.SetRange(0, int.MaxValue);
			maxDownloadSpeedSpinButton.SetRange(0, int.MaxValue);
			maxUploadSpeedSpinButton.SetRange(0, int.MaxValue);
		}
		
		private void buildLabelPage()
		{
			labelTreeView = new LabelTreeView(mainWindow, false);
			labelTreeView.sizeColumn.Visible = false;
			labelTreeView.Model = filterListStore;
			
			labelTreeView.Selection.Changed += OnLabelSelectionChanged;
			
			labelScrolledWindow.AddWithViewport(labelTreeView);
			labelScrolledWindow.ShowAll();

			//iconButton.Image = new Gtk.Image(Gtk.IconTheme.Default.LoadIcon("gtk-about", 32, 0));
			//iconButton.Sensitive = true;
			
			selectIcon = new IconEntry("", "Select an Icon");
			iconEntryBox.Add(selectIcon);
			selectIcon.ShowAll();
			
		}
		
		private void OnLabelSelectionChanged(object o, System.EventArgs args)
		{
			TreeIter iter;
			TreeModel model;

			if (((TreeSelection)o).GetSelected (out model, out iter))
			{
				TorrentLabel label = (TorrentLabel) model.GetValue (iter, 0);
				if(label.Name == "All" || label.Name == "Downloading" || label.Name == "Seeding")
					removeLabelButton.Sensitive = false;
				else
					removeLabelButton.Sensitive = true;
				return;
			}
			removeLabelButton.Sensitive = false;
		}
		
		private void buildFoldersPanel()
		{
			downloadLocationButton = new FileChooserButton("Download location", FileChooserAction.SelectFolder);
			downloadLocationButton.SetCurrentFolder(userEngineSettings.SavePath);
			
			downloadLocationButton.CurrentFolderChanged += OnDownloadLocationButtonFolderChanged;
			foldersTable.Attach(downloadLocationButton, 1, 2, 0, 1);
			downloadLocationButton.Show();
			
			torrentStorageLocationButton = new FileChooserButton("Torrage storage location", FileChooserAction.SelectFolder);
			
			torrentStorageLocationButton.SetCurrentFolder(prefSettings.TorrentStorageLocation);
			
			torrentStorageLocationButton.CurrentFolderChanged += OnTorrentStorageLocationFolderChanged;
			foldersTable.Attach(torrentStorageLocationButton, 1, 2, 1, 2);
			torrentStorageLocationButton.Show();
		}
		
		private void buildImportPanel()
		{
			importLocationButton = new FileChooserButton("Import folder to scan", FileChooserAction.SelectFolder);
			importLocationButton.SetCurrentFolder(prefSettings.ImportLocation);
			
			importLocationButton.CurrentFolderChanged += OnImportLocationFolderChanged;
			importDirectoryHbox.Add(importLocationButton);
			if(!importTorrentsCheckBox.Active)
				importLocationButton.Sensitive = false;
			importLocationButton.Show();
		}
		
		private void restorePreferences()
		{
			portSpinButton.Value = userEngineSettings.ListenPort;
			maxConnectionsSpinButton.Value = userEngineSettings.GlobalMaxConnections;
			maxDownloadSpeedSpinButton.Value = userEngineSettings.GlobalMaxDownloadSpeed;
			maxUploadSpeedSpinButton.Value = userEngineSettings.GlobalMaxUploadSpeed;
			
			upnpCheckBox.Active = prefSettings.UpnpEnabled;
			startNewTorrentsCheckBox.Active = prefSettings.StartNewTorrents;
			importTorrentsCheckBox.Active = prefSettings.ImportEnabled;
			removeOnImportCheckButton.Active = prefSettings.RemoveOnImport;
		}

		protected virtual void OnPortSpinButtonValueChanged (object sender, System.EventArgs e)
		{
			userEngineSettings.ListenPort = (int)portSpinButton.Value;
		}

		protected virtual void OnMaxConnectionsSpinButtonValueChanged (object sender, System.EventArgs e)
		{
			userEngineSettings.GlobalMaxConnections = (int) maxConnectionsSpinButton.Value;
		}
		
		private void OnDownloadLocationButtonFolderChanged (object sender, System.EventArgs e)
		{
			userEngineSettings.SavePath = downloadLocationButton.CurrentFolder;
		}
		
		private void OnTorrentStorageLocationFolderChanged(object sender, System.EventArgs e)
		{
			prefSettings.TorrentStorageLocation = torrentStorageLocationButton.Filename;
		}
		
		private void OnImportLocationFolderChanged(object sender, System.EventArgs e)
		{
			prefSettings.ImportLocation = importLocationButton.Filename;
		}

		protected virtual void OnUpnpCheckBoxToggled (object sender, System.EventArgs e)
		{
			prefSettings.UpnpEnabled = upnpCheckBox.Active;
		}

		protected virtual void OnMaxDownloadSpeedSpinButtonValueChanged (object sender, System.EventArgs e)
		{
			userEngineSettings.GlobalMaxDownloadSpeed = (int)maxDownloadSpeedSpinButton.Value;
		}

		protected virtual void OnMaxUploadSpeedSpinButtonValueChanged (object sender, System.EventArgs e)
		{
			userEngineSettings.GlobalMaxUploadSpeed = (int)maxUploadSpeedSpinButton.Value;
		}

		protected virtual void OnImportTorrentsCheckBoxClicked (object sender, System.EventArgs e)
		{
			importLocationButton.Sensitive = importTorrentsCheckBox.Active;
			prefSettings.ImportEnabled = importTorrentsCheckBox.Active;
		}

		protected virtual void OnStartNewTorrentsCheckBoxClicked (object sender, System.EventArgs e)
		{
			prefSettings.StartNewTorrents = startNewTorrentsCheckBox.Active;
		}

		protected virtual void OnIconButtonClicked (object sender, System.EventArgs e)
		{
			IconSelection iconSelection = new IconSelection();
			
			Dialog dialog = new Dialog("Select Icon", this, DialogFlags.DestroyWithParent);
			dialog.Modal = true;
			
			dialog.Add(iconSelection);
			//dialog.AddButton("Close", ResponseType.Close);
			dialog.Run();
			
			dialog.Destroy();
			
		}

		protected virtual void OnAddLabelButtonClicked (object sender, System.EventArgs e)
		{
			TorrentLabel label = new TorrentLabel(new ArrayList(), nameEntry.Text, selectIcon.Filename);;	
			labels.Add(label);
			filterListStore.AppendValues(label);
			//labelListStore.AppendValues(label.Icon, label.Name);
		}

		protected virtual void OnRemoveLabelButtonClicked (object sender, System.EventArgs e)
		{
			TreeIter iter;
			
			if(!labelTreeView.Selection.GetSelected(out iter))
				return;
			
			TorrentLabel label = (TorrentLabel) filterListStore.GetValue(iter, 0);
			filterListStore.Remove(ref iter);
			labels.Remove(label);
		}

		protected virtual void OnRemoveOnImportCheckButtonClicked (object sender, System.EventArgs e)
		{
			prefSettings.RemoveOnImport = removeOnImportCheckButton.Active;
		}

		protected virtual void OnNameEntryChanged (object sender, System.EventArgs e)
		{
			if(nameEntry.Text.Length == 0)
				addLabelButton.Sensitive = false;
			else
				addLabelButton.Sensitive = true;
		}
	}
}
