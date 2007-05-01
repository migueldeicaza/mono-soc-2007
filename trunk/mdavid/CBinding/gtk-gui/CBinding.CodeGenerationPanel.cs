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
    
    
    public partial class CodeGenerationPanel {
        
        private Gtk.Notebook notebook1;
        
        private Gtk.Table table1;
        
        private Gtk.ComboBox compilerComboBox;
        
        private Gtk.Entry extraArgsEntry;
        
        private Gtk.Label label11;
        
        private Gtk.Label label4;
        
        private Gtk.Label label5;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.SpinButton optimizationSpinButton;
        
        private Gtk.ComboBox targetComboBox;
        
        private Gtk.VBox vbox1;
        
        private Gtk.RadioButton noWarningRadio;
        
        private Gtk.RadioButton normalWarningRadio;
        
        private Gtk.RadioButton allWarningRadio;
        
        private Gtk.Label label1;
        
        private Gtk.Table table2;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Button addLibButton;
        
        private Gtk.VSeparator vseparator1;
        
        private Gtk.Button removeLibButton;
        
        private Gtk.Label label8;
        
        private Gtk.Entry libAddEntry;
        
        private Gtk.TextView libTextView;
        
        private Gtk.Label label2;
        
        private Gtk.VPaned vpaned1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label label9;
        
        private Gtk.TextView includePathTextView;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Entry includePathEntry;
        
        private Gtk.Button includePathAddButton;
        
        private Gtk.Button includePathRemoveButton;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Label label10;
        
        private Gtk.TextView libPathTextView;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Entry libPathEntry;
        
        private Gtk.Button libPathAddButton;
        
        private Gtk.Button libPathRemoveButton;
        
        private Gtk.Label label3;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget CBinding.CodeGenerationPanel
            Stetic.BinContainer.Attach(this);
            this.Events = ((Gdk.EventMask)(0));
            this.Name = "CBinding.CodeGenerationPanel";
            // Container child CBinding.CodeGenerationPanel.Gtk.Container+ContainerChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Events = ((Gdk.EventMask)(0));
            this.notebook1.Name = "notebook1";
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table1 = new Gtk.Table(((uint)(5)), ((uint)(2)), false);
            this.table1.Events = ((Gdk.EventMask)(0));
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(10));
            this.table1.ColumnSpacing = ((uint)(10));
            // Container child table1.Gtk.Table+TableChild
            this.compilerComboBox = Gtk.ComboBox.NewText();
            this.compilerComboBox.Events = ((Gdk.EventMask)(0));
            this.compilerComboBox.Name = "compilerComboBox";
            this.compilerComboBox.Active = 0;
            this.table1.Add(this.compilerComboBox);
            Gtk.Table.TableChild w1 = ((Gtk.Table.TableChild)(this.table1[this.compilerComboBox]));
            w1.LeftAttach = ((uint)(1));
            w1.RightAttach = ((uint)(2));
            w1.XOptions = ((Gtk.AttachOptions)(4));
            w1.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.extraArgsEntry = new Gtk.Entry();
            this.extraArgsEntry.CanFocus = true;
            this.extraArgsEntry.Events = ((Gdk.EventMask)(0));
            this.extraArgsEntry.Name = "extraArgsEntry";
            this.extraArgsEntry.IsEditable = true;
            this.extraArgsEntry.InvisibleChar = '●';
            this.table1.Add(this.extraArgsEntry);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.extraArgsEntry]));
            w2.TopAttach = ((uint)(4));
            w2.BottomAttach = ((uint)(5));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label11 = new Gtk.Label();
            this.label11.Events = ((Gdk.EventMask)(0));
            this.label11.Name = "label11";
            this.label11.Xpad = 10;
            this.label11.Xalign = 0F;
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("Compiler:");
            this.table1.Add(this.label11);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table1[this.label11]));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Events = ((Gdk.EventMask)(0));
            this.label4.Name = "label4";
            this.label4.Xpad = 10;
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Warning Level:");
            this.table1.Add(this.label4);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.label4]));
            w4.TopAttach = ((uint)(1));
            w4.BottomAttach = ((uint)(2));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label5 = new Gtk.Label();
            this.label5.Events = ((Gdk.EventMask)(0));
            this.label5.Name = "label5";
            this.label5.Xpad = 10;
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Optimization Level:");
            this.table1.Add(this.label5);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.label5]));
            w5.TopAttach = ((uint)(2));
            w5.BottomAttach = ((uint)(3));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label6 = new Gtk.Label();
            this.label6.Events = ((Gdk.EventMask)(0));
            this.label6.Name = "label6";
            this.label6.Xpad = 10;
            this.label6.Xalign = 0F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Target:");
            this.table1.Add(this.label6);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.label6]));
            w6.TopAttach = ((uint)(3));
            w6.BottomAttach = ((uint)(4));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label7 = new Gtk.Label();
            this.label7.Events = ((Gdk.EventMask)(0));
            this.label7.Name = "label7";
            this.label7.Xpad = 10;
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Extra Compiler Arguments:");
            this.table1.Add(this.label7);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table1[this.label7]));
            w7.TopAttach = ((uint)(4));
            w7.BottomAttach = ((uint)(5));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.optimizationSpinButton = new Gtk.SpinButton(0, 3, 1);
            this.optimizationSpinButton.CanFocus = true;
            this.optimizationSpinButton.Events = ((Gdk.EventMask)(0));
            this.optimizationSpinButton.Name = "optimizationSpinButton";
            this.optimizationSpinButton.Adjustment.PageIncrement = 10;
            this.optimizationSpinButton.ClimbRate = 1;
            this.optimizationSpinButton.Numeric = true;
            this.table1.Add(this.optimizationSpinButton);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table1[this.optimizationSpinButton]));
            w8.TopAttach = ((uint)(2));
            w8.BottomAttach = ((uint)(3));
            w8.LeftAttach = ((uint)(1));
            w8.RightAttach = ((uint)(2));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.targetComboBox = Gtk.ComboBox.NewText();
            this.targetComboBox.AppendText(Mono.Unix.Catalog.GetString("Executable"));
            this.targetComboBox.AppendText(Mono.Unix.Catalog.GetString("Static Library"));
            this.targetComboBox.AppendText(Mono.Unix.Catalog.GetString("Shared Object"));
            this.targetComboBox.Events = ((Gdk.EventMask)(0));
            this.targetComboBox.Name = "targetComboBox";
            this.table1.Add(this.targetComboBox);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table1[this.targetComboBox]));
            w9.TopAttach = ((uint)(3));
            w9.BottomAttach = ((uint)(4));
            w9.LeftAttach = ((uint)(1));
            w9.RightAttach = ((uint)(2));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Events = ((Gdk.EventMask)(0));
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 1;
            // Container child vbox1.Gtk.Box+BoxChild
            this.noWarningRadio = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("no warnings"));
            this.noWarningRadio.CanFocus = true;
            this.noWarningRadio.Events = ((Gdk.EventMask)(0));
            this.noWarningRadio.Name = "noWarningRadio";
            this.noWarningRadio.Active = true;
            this.noWarningRadio.DrawIndicator = true;
            this.noWarningRadio.UseUnderline = true;
            this.noWarningRadio.Group = new GLib.SList(System.IntPtr.Zero);
            this.vbox1.Add(this.noWarningRadio);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox1[this.noWarningRadio]));
            w10.Position = 0;
            w10.Expand = false;
            w10.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.normalWarningRadio = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("normal"));
            this.normalWarningRadio.CanFocus = true;
            this.normalWarningRadio.Events = ((Gdk.EventMask)(0));
            this.normalWarningRadio.Name = "normalWarningRadio";
            this.normalWarningRadio.DrawIndicator = true;
            this.normalWarningRadio.UseUnderline = true;
            this.normalWarningRadio.Group = this.noWarningRadio.Group;
            this.vbox1.Add(this.normalWarningRadio);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox1[this.normalWarningRadio]));
            w11.Position = 1;
            w11.Expand = false;
            w11.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.allWarningRadio = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("all"));
            this.allWarningRadio.CanFocus = true;
            this.allWarningRadio.Events = ((Gdk.EventMask)(0));
            this.allWarningRadio.Name = "allWarningRadio";
            this.allWarningRadio.DrawIndicator = true;
            this.allWarningRadio.UseUnderline = true;
            this.allWarningRadio.Group = this.noWarningRadio.Group;
            this.vbox1.Add(this.allWarningRadio);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.vbox1[this.allWarningRadio]));
            w12.Position = 2;
            w12.Expand = false;
            w12.Fill = false;
            this.table1.Add(this.vbox1);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table1[this.vbox1]));
            w13.TopAttach = ((uint)(1));
            w13.BottomAttach = ((uint)(2));
            w13.LeftAttach = ((uint)(1));
            w13.RightAttach = ((uint)(2));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table1);
            Gtk.Notebook.NotebookChild w14 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table1]));
            w14.TabExpand = false;
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Events = ((Gdk.EventMask)(0));
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Code Generation");
            this.notebook1.SetTabLabel(this.table1, this.label1);
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table2 = new Gtk.Table(((uint)(2)), ((uint)(3)), false);
            this.table2.Events = ((Gdk.EventMask)(0));
            this.table2.Name = "table2";
            this.table2.RowSpacing = ((uint)(10));
            this.table2.ColumnSpacing = ((uint)(10));
            // Container child table2.Gtk.Table+TableChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Events = ((Gdk.EventMask)(0));
            this.hbox1.Name = "hbox1";
            // Container child hbox1.Gtk.Box+BoxChild
            this.addLibButton = new Gtk.Button();
            this.addLibButton.CanFocus = true;
            this.addLibButton.Events = ((Gdk.EventMask)(0));
            this.addLibButton.Name = "addLibButton";
            this.addLibButton.UseUnderline = true;
            this.addLibButton.Label = Mono.Unix.Catalog.GetString("Add");
            this.hbox1.Add(this.addLibButton);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox1[this.addLibButton]));
            w15.Position = 0;
            w15.Expand = false;
            w15.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vseparator1 = new Gtk.VSeparator();
            this.vseparator1.Events = ((Gdk.EventMask)(0));
            this.vseparator1.Name = "vseparator1";
            this.hbox1.Add(this.vseparator1);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.hbox1[this.vseparator1]));
            w16.Position = 1;
            w16.Expand = false;
            w16.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.removeLibButton = new Gtk.Button();
            this.removeLibButton.CanFocus = true;
            this.removeLibButton.Events = ((Gdk.EventMask)(0));
            this.removeLibButton.Name = "removeLibButton";
            this.removeLibButton.UseUnderline = true;
            this.removeLibButton.Label = Mono.Unix.Catalog.GetString("Remove");
            this.hbox1.Add(this.removeLibButton);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox1[this.removeLibButton]));
            w17.Position = 2;
            w17.Expand = false;
            w17.Fill = false;
            this.table2.Add(this.hbox1);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table2[this.hbox1]));
            w18.LeftAttach = ((uint)(2));
            w18.RightAttach = ((uint)(3));
            w18.XOptions = ((Gtk.AttachOptions)(4));
            w18.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label8 = new Gtk.Label();
            this.label8.Events = ((Gdk.EventMask)(0));
            this.label8.Name = "label8";
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Library:");
            this.table2.Add(this.label8);
            Gtk.Table.TableChild w19 = ((Gtk.Table.TableChild)(this.table2[this.label8]));
            w19.XOptions = ((Gtk.AttachOptions)(4));
            w19.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.libAddEntry = new Gtk.Entry();
            this.libAddEntry.CanFocus = true;
            this.libAddEntry.Events = ((Gdk.EventMask)(0));
            this.libAddEntry.Name = "libAddEntry";
            this.libAddEntry.IsEditable = true;
            this.libAddEntry.InvisibleChar = '●';
            this.table2.Add(this.libAddEntry);
            Gtk.Table.TableChild w20 = ((Gtk.Table.TableChild)(this.table2[this.libAddEntry]));
            w20.LeftAttach = ((uint)(1));
            w20.RightAttach = ((uint)(2));
            w20.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.libTextView = new Gtk.TextView();
            this.libTextView.CanFocus = true;
            this.libTextView.Events = ((Gdk.EventMask)(0));
            this.libTextView.Name = "libTextView";
            this.libTextView.Editable = false;
            this.libTextView.CursorVisible = false;
            this.libTextView.LeftMargin = 10;
            this.table2.Add(this.libTextView);
            Gtk.Table.TableChild w21 = ((Gtk.Table.TableChild)(this.table2[this.libTextView]));
            w21.TopAttach = ((uint)(1));
            w21.BottomAttach = ((uint)(2));
            w21.LeftAttach = ((uint)(1));
            w21.RightAttach = ((uint)(2));
            this.notebook1.Add(this.table2);
            Gtk.Notebook.NotebookChild w22 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table2]));
            w22.Position = 1;
            w22.TabExpand = false;
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Events = ((Gdk.EventMask)(0));
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Libraries");
            this.notebook1.SetTabLabel(this.table2, this.label2);
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vpaned1 = new Gtk.VPaned();
            this.vpaned1.CanFocus = true;
            this.vpaned1.Events = ((Gdk.EventMask)(0));
            this.vpaned1.Name = "vpaned1";
            this.vpaned1.Position = 172;
            // Container child vpaned1.Gtk.Paned+PanedChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Events = ((Gdk.EventMask)(0));
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 10;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label9 = new Gtk.Label();
            this.label9.Events = ((Gdk.EventMask)(0));
            this.label9.Name = "label9";
            this.label9.Xpad = 10;
            this.label9.Xalign = 0F;
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("Include:");
            this.vbox2.Add(this.label9);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.vbox2[this.label9]));
            w23.Position = 0;
            w23.Expand = false;
            w23.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.includePathTextView = new Gtk.TextView();
            this.includePathTextView.CanFocus = true;
            this.includePathTextView.Events = ((Gdk.EventMask)(0));
            this.includePathTextView.Name = "includePathTextView";
            this.includePathTextView.Editable = false;
            this.includePathTextView.CursorVisible = false;
            this.includePathTextView.LeftMargin = 10;
            this.vbox2.Add(this.includePathTextView);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.vbox2[this.includePathTextView]));
            w24.Position = 1;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Events = ((Gdk.EventMask)(0));
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 5;
            // Container child hbox2.Gtk.Box+BoxChild
            this.includePathEntry = new Gtk.Entry();
            this.includePathEntry.CanFocus = true;
            this.includePathEntry.Events = ((Gdk.EventMask)(0));
            this.includePathEntry.Name = "includePathEntry";
            this.includePathEntry.IsEditable = true;
            this.includePathEntry.InvisibleChar = '●';
            this.hbox2.Add(this.includePathEntry);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.hbox2[this.includePathEntry]));
            w25.Position = 0;
            // Container child hbox2.Gtk.Box+BoxChild
            this.includePathAddButton = new Gtk.Button();
            this.includePathAddButton.CanFocus = true;
            this.includePathAddButton.Events = ((Gdk.EventMask)(0));
            this.includePathAddButton.Name = "includePathAddButton";
            this.includePathAddButton.UseUnderline = true;
            this.includePathAddButton.Label = Mono.Unix.Catalog.GetString("Add");
            this.hbox2.Add(this.includePathAddButton);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox2[this.includePathAddButton]));
            w26.Position = 1;
            w26.Expand = false;
            w26.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.includePathRemoveButton = new Gtk.Button();
            this.includePathRemoveButton.CanFocus = true;
            this.includePathRemoveButton.Events = ((Gdk.EventMask)(0));
            this.includePathRemoveButton.Name = "includePathRemoveButton";
            this.includePathRemoveButton.UseUnderline = true;
            this.includePathRemoveButton.Label = Mono.Unix.Catalog.GetString("Remove");
            this.hbox2.Add(this.includePathRemoveButton);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.hbox2[this.includePathRemoveButton]));
            w27.Position = 2;
            w27.Expand = false;
            w27.Fill = false;
            this.vbox2.Add(this.hbox2);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
            w28.Position = 2;
            w28.Expand = false;
            w28.Fill = false;
            this.vpaned1.Add(this.vbox2);
            Gtk.Paned.PanedChild w29 = ((Gtk.Paned.PanedChild)(this.vpaned1[this.vbox2]));
            w29.Resize = false;
            // Container child vpaned1.Gtk.Paned+PanedChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Events = ((Gdk.EventMask)(0));
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 10;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label10 = new Gtk.Label();
            this.label10.Events = ((Gdk.EventMask)(0));
            this.label10.Name = "label10";
            this.label10.Xpad = 10;
            this.label10.Xalign = 0F;
            this.label10.LabelProp = Mono.Unix.Catalog.GetString("Library:");
            this.vbox3.Add(this.label10);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.vbox3[this.label10]));
            w30.Position = 0;
            w30.Expand = false;
            w30.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.libPathTextView = new Gtk.TextView();
            this.libPathTextView.CanFocus = true;
            this.libPathTextView.Events = ((Gdk.EventMask)(0));
            this.libPathTextView.Name = "libPathTextView";
            this.libPathTextView.Editable = false;
            this.libPathTextView.CursorVisible = false;
            this.libPathTextView.LeftMargin = 10;
            this.vbox3.Add(this.libPathTextView);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.vbox3[this.libPathTextView]));
            w31.Position = 1;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Events = ((Gdk.EventMask)(0));
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 5;
            // Container child hbox3.Gtk.Box+BoxChild
            this.libPathEntry = new Gtk.Entry();
            this.libPathEntry.CanFocus = true;
            this.libPathEntry.Events = ((Gdk.EventMask)(0));
            this.libPathEntry.Name = "libPathEntry";
            this.libPathEntry.IsEditable = true;
            this.libPathEntry.InvisibleChar = '●';
            this.hbox3.Add(this.libPathEntry);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.hbox3[this.libPathEntry]));
            w32.Position = 0;
            // Container child hbox3.Gtk.Box+BoxChild
            this.libPathAddButton = new Gtk.Button();
            this.libPathAddButton.CanFocus = true;
            this.libPathAddButton.Events = ((Gdk.EventMask)(0));
            this.libPathAddButton.Name = "libPathAddButton";
            this.libPathAddButton.UseUnderline = true;
            this.libPathAddButton.Label = Mono.Unix.Catalog.GetString("Add");
            this.hbox3.Add(this.libPathAddButton);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.hbox3[this.libPathAddButton]));
            w33.Position = 1;
            w33.Expand = false;
            w33.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.libPathRemoveButton = new Gtk.Button();
            this.libPathRemoveButton.CanFocus = true;
            this.libPathRemoveButton.Events = ((Gdk.EventMask)(0));
            this.libPathRemoveButton.Name = "libPathRemoveButton";
            this.libPathRemoveButton.UseUnderline = true;
            this.libPathRemoveButton.Label = Mono.Unix.Catalog.GetString("Remove");
            this.hbox3.Add(this.libPathRemoveButton);
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.hbox3[this.libPathRemoveButton]));
            w34.Position = 2;
            w34.Expand = false;
            w34.Fill = false;
            this.vbox3.Add(this.hbox3);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox3]));
            w35.Position = 2;
            w35.Expand = false;
            w35.Fill = false;
            this.vpaned1.Add(this.vbox3);
            this.notebook1.Add(this.vpaned1);
            Gtk.Notebook.NotebookChild w37 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.vpaned1]));
            w37.Position = 2;
            w37.TabExpand = false;
            // Notebook tab
            this.label3 = new Gtk.Label();
            this.label3.Events = ((Gdk.EventMask)(0));
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Paths");
            this.notebook1.SetTabLabel(this.vpaned1, this.label3);
            this.Add(this.notebook1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
        }
    }
}
