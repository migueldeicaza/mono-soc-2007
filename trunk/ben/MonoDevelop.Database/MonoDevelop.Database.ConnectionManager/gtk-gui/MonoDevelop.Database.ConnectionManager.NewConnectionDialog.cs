// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace MonoDevelop.Database.ConnectionManager {
    
    
    public partial class NewConnectionDialog {
        
        private Gtk.VBox vbox;
        
        private Gtk.RadioButton radioGenerated;
        
        private Gtk.Alignment alignmentGenerated;
        
        private Gtk.Table table;
        
        private Gtk.CheckButton checkPooling;
        
        private Gtk.ComboBoxEntry comboDatabase;
        
        private Gtk.ComboBox comboProvider;
        
        private Gtk.Entry entryName;
        
        private Gtk.Entry entryPassword;
        
        private Gtk.Entry entryServer;
        
        private Gtk.Entry entryUsername;
        
        private Gtk.Label label1;
        
        private Gtk.Label label2;
        
        private Gtk.Label label3;
        
        private Gtk.Label label4;
        
        private Gtk.Label label5;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.Label label8;
        
        private Gtk.Label label9;
        
        private Gtk.SpinButton spinMaxPool;
        
        private Gtk.SpinButton spinMinPool;
        
        private Gtk.SpinButton spinPort;
        
        private Gtk.RadioButton radioUseCustom;
        
        private Gtk.Alignment alignmentCustom;
        
        private Gtk.ScrolledWindow scrolledwindow;
        
        private Gtk.TextView textConnectionString;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget MonoDevelop.Database.ConnectionManager.NewConnectionDialog
            this.Name = "MonoDevelop.Database.ConnectionManager.NewConnectionDialog";
            this.Title = Mono.Unix.Catalog.GetString("Add Connection");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.HasSeparator = false;
            // Internal child MonoDevelop.Database.ConnectionManager.NewConnectionDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.vbox = new Gtk.VBox();
            this.vbox.Name = "vbox";
            this.vbox.Spacing = 6;
            this.vbox.BorderWidth = ((uint)(5));
            // Container child vbox.Gtk.Box+BoxChild
            this.radioGenerated = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Use generated connection string"));
            this.radioGenerated.CanFocus = true;
            this.radioGenerated.Name = "radioGenerated";
            this.radioGenerated.Active = true;
            this.radioGenerated.DrawIndicator = true;
            this.radioGenerated.UseUnderline = true;
            this.radioGenerated.Group = new GLib.SList(System.IntPtr.Zero);
            this.vbox.Add(this.radioGenerated);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox[this.radioGenerated]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox.Gtk.Box+BoxChild
            this.alignmentGenerated = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignmentGenerated.Name = "alignmentGenerated";
            this.alignmentGenerated.LeftPadding = ((uint)(10));
            // Container child alignmentGenerated.Gtk.Container+ContainerChild
            this.table = new Gtk.Table(((uint)(10)), ((uint)(2)), false);
            this.table.Name = "table";
            this.table.RowSpacing = ((uint)(5));
            this.table.ColumnSpacing = ((uint)(5));
            // Container child table.Gtk.Table+TableChild
            this.checkPooling = new Gtk.CheckButton();
            this.checkPooling.CanFocus = true;
            this.checkPooling.Name = "checkPooling";
            this.checkPooling.Label = Mono.Unix.Catalog.GetString("Use connection pooling");
            this.checkPooling.Active = true;
            this.checkPooling.DrawIndicator = true;
            this.checkPooling.UseUnderline = true;
            this.table.Add(this.checkPooling);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table[this.checkPooling]));
            w3.TopAttach = ((uint)(7));
            w3.BottomAttach = ((uint)(8));
            w3.RightAttach = ((uint)(2));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.comboDatabase = new Gtk.ComboBoxEntry();
            this.comboDatabase.Name = "comboDatabase";
            this.table.Add(this.comboDatabase);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table[this.comboDatabase]));
            w4.TopAttach = ((uint)(4));
            w4.BottomAttach = ((uint)(5));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.comboProvider = new Gtk.ComboBox();
            this.comboProvider.Name = "comboProvider";
            this.table.Add(this.comboProvider);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table[this.comboProvider]));
            w5.LeftAttach = ((uint)(1));
            w5.RightAttach = ((uint)(2));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.entryName = new Gtk.Entry();
            this.entryName.CanFocus = true;
            this.entryName.Name = "entryName";
            this.entryName.IsEditable = true;
            this.entryName.InvisibleChar = '●';
            this.table.Add(this.entryName);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table[this.entryName]));
            w6.TopAttach = ((uint)(1));
            w6.BottomAttach = ((uint)(2));
            w6.LeftAttach = ((uint)(1));
            w6.RightAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.entryPassword = new Gtk.Entry();
            this.entryPassword.CanFocus = true;
            this.entryPassword.Name = "entryPassword";
            this.entryPassword.IsEditable = true;
            this.entryPassword.InvisibleChar = '●';
            this.table.Add(this.entryPassword);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table[this.entryPassword]));
            w7.TopAttach = ((uint)(6));
            w7.BottomAttach = ((uint)(7));
            w7.LeftAttach = ((uint)(1));
            w7.RightAttach = ((uint)(2));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.entryServer = new Gtk.Entry();
            this.entryServer.CanFocus = true;
            this.entryServer.Name = "entryServer";
            this.entryServer.IsEditable = true;
            this.entryServer.InvisibleChar = '●';
            this.table.Add(this.entryServer);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table[this.entryServer]));
            w8.TopAttach = ((uint)(2));
            w8.BottomAttach = ((uint)(3));
            w8.LeftAttach = ((uint)(1));
            w8.RightAttach = ((uint)(2));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.entryUsername = new Gtk.Entry();
            this.entryUsername.CanFocus = true;
            this.entryUsername.Name = "entryUsername";
            this.entryUsername.IsEditable = true;
            this.entryUsername.InvisibleChar = '●';
            this.table.Add(this.entryUsername);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table[this.entryUsername]));
            w9.TopAttach = ((uint)(5));
            w9.BottomAttach = ((uint)(6));
            w9.LeftAttach = ((uint)(1));
            w9.RightAttach = ((uint)(2));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Provider");
            this.table.Add(this.label1);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table[this.label1]));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.Xalign = 0F;
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Name");
            this.table.Add(this.label2);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table[this.label2]));
            w11.TopAttach = ((uint)(1));
            w11.BottomAttach = ((uint)(2));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xalign = 0F;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Server");
            this.table.Add(this.label3);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table[this.label3]));
            w12.TopAttach = ((uint)(2));
            w12.BottomAttach = ((uint)(3));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Port");
            this.table.Add(this.label4);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table[this.label4]));
            w13.TopAttach = ((uint)(3));
            w13.BottomAttach = ((uint)(4));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Database");
            this.table.Add(this.label5);
            Gtk.Table.TableChild w14 = ((Gtk.Table.TableChild)(this.table[this.label5]));
            w14.TopAttach = ((uint)(4));
            w14.BottomAttach = ((uint)(5));
            w14.XOptions = ((Gtk.AttachOptions)(4));
            w14.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Xalign = 0F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Username");
            this.table.Add(this.label6);
            Gtk.Table.TableChild w15 = ((Gtk.Table.TableChild)(this.table[this.label6]));
            w15.TopAttach = ((uint)(5));
            w15.BottomAttach = ((uint)(6));
            w15.XOptions = ((Gtk.AttachOptions)(4));
            w15.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Password");
            this.table.Add(this.label7);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table[this.label7]));
            w16.TopAttach = ((uint)(6));
            w16.BottomAttach = ((uint)(7));
            w16.XOptions = ((Gtk.AttachOptions)(4));
            w16.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Min pool size");
            this.table.Add(this.label8);
            Gtk.Table.TableChild w17 = ((Gtk.Table.TableChild)(this.table[this.label8]));
            w17.TopAttach = ((uint)(8));
            w17.BottomAttach = ((uint)(9));
            w17.XOptions = ((Gtk.AttachOptions)(4));
            w17.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("Max pool size");
            this.table.Add(this.label9);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table[this.label9]));
            w18.TopAttach = ((uint)(9));
            w18.BottomAttach = ((uint)(10));
            w18.XOptions = ((Gtk.AttachOptions)(4));
            w18.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.spinMaxPool = new Gtk.SpinButton(1, 100, 1);
            this.spinMaxPool.CanFocus = true;
            this.spinMaxPool.Name = "spinMaxPool";
            this.spinMaxPool.Adjustment.PageIncrement = 10;
            this.spinMaxPool.ClimbRate = 1;
            this.spinMaxPool.Numeric = true;
            this.spinMaxPool.Value = 20;
            this.table.Add(this.spinMaxPool);
            Gtk.Table.TableChild w19 = ((Gtk.Table.TableChild)(this.table[this.spinMaxPool]));
            w19.TopAttach = ((uint)(9));
            w19.BottomAttach = ((uint)(10));
            w19.LeftAttach = ((uint)(1));
            w19.RightAttach = ((uint)(2));
            w19.XOptions = ((Gtk.AttachOptions)(4));
            w19.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.spinMinPool = new Gtk.SpinButton(1, 100, 1);
            this.spinMinPool.CanFocus = true;
            this.spinMinPool.Name = "spinMinPool";
            this.spinMinPool.Adjustment.PageIncrement = 10;
            this.spinMinPool.ClimbRate = 1;
            this.spinMinPool.Numeric = true;
            this.spinMinPool.Value = 1;
            this.table.Add(this.spinMinPool);
            Gtk.Table.TableChild w20 = ((Gtk.Table.TableChild)(this.table[this.spinMinPool]));
            w20.TopAttach = ((uint)(8));
            w20.BottomAttach = ((uint)(9));
            w20.LeftAttach = ((uint)(1));
            w20.RightAttach = ((uint)(2));
            w20.XOptions = ((Gtk.AttachOptions)(4));
            w20.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table.Gtk.Table+TableChild
            this.spinPort = new Gtk.SpinButton(1, 65535, 1);
            this.spinPort.CanFocus = true;
            this.spinPort.Name = "spinPort";
            this.spinPort.Adjustment.PageIncrement = 10;
            this.spinPort.ClimbRate = 1;
            this.spinPort.Numeric = true;
            this.spinPort.Value = 1;
            this.table.Add(this.spinPort);
            Gtk.Table.TableChild w21 = ((Gtk.Table.TableChild)(this.table[this.spinPort]));
            w21.TopAttach = ((uint)(3));
            w21.BottomAttach = ((uint)(4));
            w21.LeftAttach = ((uint)(1));
            w21.RightAttach = ((uint)(2));
            w21.XOptions = ((Gtk.AttachOptions)(4));
            w21.YOptions = ((Gtk.AttachOptions)(4));
            this.alignmentGenerated.Add(this.table);
            this.vbox.Add(this.alignmentGenerated);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.vbox[this.alignmentGenerated]));
            w23.Position = 1;
            w23.Expand = false;
            w23.Fill = false;
            // Container child vbox.Gtk.Box+BoxChild
            this.radioUseCustom = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Use custom connection string"));
            this.radioUseCustom.CanFocus = true;
            this.radioUseCustom.Name = "radioUseCustom";
            this.radioUseCustom.DrawIndicator = true;
            this.radioUseCustom.UseUnderline = true;
            this.radioUseCustom.Group = this.radioGenerated.Group;
            this.vbox.Add(this.radioUseCustom);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.vbox[this.radioUseCustom]));
            w24.Position = 2;
            w24.Expand = false;
            w24.Fill = false;
            // Container child vbox.Gtk.Box+BoxChild
            this.alignmentCustom = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignmentCustom.Name = "alignmentCustom";
            this.alignmentCustom.LeftPadding = ((uint)(10));
            // Container child alignmentCustom.Gtk.Container+ContainerChild
            this.scrolledwindow = new Gtk.ScrolledWindow();
            this.scrolledwindow.CanFocus = true;
            this.scrolledwindow.Name = "scrolledwindow";
            this.scrolledwindow.VscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow.HscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow.Gtk.Container+ContainerChild
            this.textConnectionString = new Gtk.TextView();
            this.textConnectionString.CanFocus = true;
            this.textConnectionString.Name = "textConnectionString";
            this.scrolledwindow.Add(this.textConnectionString);
            this.alignmentCustom.Add(this.scrolledwindow);
            this.vbox.Add(this.alignmentCustom);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.vbox[this.alignmentCustom]));
            w27.Position = 3;
            w1.Add(this.vbox);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(w1[this.vbox]));
            w28.Position = 0;
            // Internal child MonoDevelop.Database.ConnectionManager.NewConnectionDialog.ActionArea
            Gtk.HButtonBox w29 = this.ActionArea;
            w29.Name = "dialog1_ActionArea";
            w29.Spacing = 6;
            w29.BorderWidth = ((uint)(5));
            w29.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w30 = ((Gtk.ButtonBox.ButtonBoxChild)(w29[this.buttonCancel]));
            w30.Expand = false;
            w30.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.Sensitive = false;
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w31 = ((Gtk.ButtonBox.ButtonBoxChild)(w29[this.buttonOk]));
            w31.Position = 1;
            w31.Expand = false;
            w31.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 427;
            this.DefaultHeight = 556;
            this.Show();
            this.radioGenerated.Toggled += new System.EventHandler(this.RadioGeneratedChange);
            this.spinPort.Changed += new System.EventHandler(this.PortChanged);
            this.entryUsername.Changed += new System.EventHandler(this.UsernameChanged);
            this.entryServer.Changed += new System.EventHandler(this.ServerChanged);
            this.entryPassword.Changed += new System.EventHandler(this.PasswordChanged);
            this.entryName.Changed += new System.EventHandler(this.NameChanged);
            this.comboProvider.Changed += new System.EventHandler(this.ComboProviderChanged);
            this.comboDatabase.Changed += new System.EventHandler(this.DatabaseChanged);
            this.checkPooling.Activated += new System.EventHandler(this.CheckPoolingActivated);
            this.buttonCancel.Clicked += new System.EventHandler(this.CancelClicked);
            this.buttonOk.Clicked += new System.EventHandler(this.OkClicked);
        }
    }
}