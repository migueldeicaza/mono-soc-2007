// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace CBinding {
    
    
    public partial class EditPackagesDialog {
        
        private Gtk.VPaned vpaned1;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.TreeView packageTreeView;
        
        private Gtk.Table table1;
        
        private Gtk.Label label1;
        
        private Gtk.ScrolledWindow scrolledwindow2;
        
        private Gtk.TreeView selectedPackagesTreeView;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Button removeButton;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget CBinding.EditPackagesDialog
            this.Name = "CBinding.EditPackagesDialog";
            this.Title = Mono.Unix.Catalog.GetString("Edit packages");
            this.Modal = true;
            // Internal child CBinding.EditPackagesDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.vpaned1 = new Gtk.VPaned();
            this.vpaned1.CanFocus = true;
            this.vpaned1.Name = "vpaned1";
            this.vpaned1.Position = 183;
            this.vpaned1.BorderWidth = ((uint)(6));
            // Container child vpaned1.Gtk.Paned+PanedChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.VscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow1.HscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            this.packageTreeView = new Gtk.TreeView();
            this.packageTreeView.CanFocus = true;
            this.packageTreeView.Name = "packageTreeView";
            this.scrolledwindow1.Add(this.packageTreeView);
            this.vpaned1.Add(this.scrolledwindow1);
            Gtk.Paned.PanedChild w3 = ((Gtk.Paned.PanedChild)(this.vpaned1[this.scrolledwindow1]));
            w3.Resize = false;
            // Container child vpaned1.Gtk.Paned+PanedChild
            this.table1 = new Gtk.Table(((uint)(2)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.Yalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Selected packages:");
            this.table1.Add(this.label1);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.label1]));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.scrolledwindow2 = new Gtk.ScrolledWindow();
            this.scrolledwindow2.CanFocus = true;
            this.scrolledwindow2.Name = "scrolledwindow2";
            this.scrolledwindow2.VscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow2.HscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow2.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow2.Gtk.Container+ContainerChild
            this.selectedPackagesTreeView = new Gtk.TreeView();
            this.selectedPackagesTreeView.CanFocus = true;
            this.selectedPackagesTreeView.Name = "selectedPackagesTreeView";
            this.scrolledwindow2.Add(this.selectedPackagesTreeView);
            this.table1.Add(this.scrolledwindow2);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.scrolledwindow2]));
            w6.TopAttach = ((uint)(1));
            w6.BottomAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.removeButton = new Gtk.Button();
            this.removeButton.CanFocus = true;
            this.removeButton.Name = "removeButton";
            this.removeButton.UseUnderline = true;
            // Container child removeButton.Gtk.Container+ContainerChild
            Gtk.Alignment w7 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            w7.Name = "GtkAlignment";
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w8 = new Gtk.HBox();
            w8.Name = "GtkHBox";
            w8.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w9 = new Gtk.Image();
            w9.Name = "image1";
            w9.Pixbuf = Stetic.IconLoader.LoadIcon("gtk-remove", 16);
            w8.Add(w9);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w11 = new Gtk.Label();
            w11.Name = "GtkLabel";
            w11.LabelProp = "";
            w8.Add(w11);
            w7.Add(w8);
            this.removeButton.Add(w7);
            this.vbox2.Add(this.removeButton);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.vbox2[this.removeButton]));
            w15.Position = 0;
            w15.Expand = false;
            w15.Fill = false;
            this.table1.Add(this.vbox2);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table1[this.vbox2]));
            w16.TopAttach = ((uint)(1));
            w16.BottomAttach = ((uint)(2));
            w16.LeftAttach = ((uint)(1));
            w16.RightAttach = ((uint)(2));
            w16.XOptions = ((Gtk.AttachOptions)(4));
            this.vpaned1.Add(this.table1);
            w1.Add(this.vpaned1);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(w1[this.vpaned1]));
            w18.Position = 0;
            w18.Padding = ((uint)(3));
            // Internal child CBinding.EditPackagesDialog.ActionArea
            Gtk.HButtonBox w19 = this.ActionArea;
            w19.Name = "dialog1_ActionArea";
            w19.Spacing = 6;
            w19.BorderWidth = ((uint)(5));
            w19.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w20 = ((Gtk.ButtonBox.ButtonBoxChild)(w19[this.buttonCancel]));
            w20.Expand = false;
            w20.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w21 = ((Gtk.ButtonBox.ButtonBoxChild)(w19[this.buttonOk]));
            w21.Position = 1;
            w21.Expand = false;
            w21.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 580;
            this.DefaultHeight = 449;
            this.Show();
        }
    }
}
