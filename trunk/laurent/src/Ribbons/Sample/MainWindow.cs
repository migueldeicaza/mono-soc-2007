using System;
using Cairo;
using Gtk;
using Ribbons;

namespace Sample
{
	public class MainWindow : SyntheticWindow
	{
		protected bool composeAvailable = false;
		
		protected Ribbon ribbon;
		protected RibbonGroup group0, group1, group2;
		
		protected Label pageLabel1;
		
		public MainWindow() : base (WindowType.Toplevel)
		{
			AddEvents ((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.PointerMotionMask));
			
			VBox master = new VBox ();
			master.AddEvents ((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.PointerMotionMask));
			
			Title = "Ribbons Sample";
			AppPaintable = true;
			
			Ribbons.Button button0 = new Ribbons.Button ("Hello World");
			
			group0 = new RibbonGroup ();
			group0.Label = "Summer of Code";
			group0.Child = button0;
			group0.Expand += onClick;
			
			Menu openMenu = new Menu ();
			
			Ribbons.Button open = Ribbons.Button.FromStockIcon (Gtk.Stock.Open, "Open", false);
			open.DropDownMenu = openMenu;
			open.Clicked += onClick;
			
			Ribbons.ToolPack toolPack = new Ribbons.ToolPack ();
			toolPack.AppendButton (Ribbons.Button.FromStockIcon (Gtk.Stock.New, "New", false));
			toolPack.AppendButton (open);
			toolPack.AppendButton (Ribbons.Button.FromStockIcon (Gtk.Stock.Save, "Save", false));
			
			Ribbons.ToolBox box0 = new ToolBox ();
			box0.Append (toolPack);
			
			group1 = new RibbonGroup ();
			group1.Label = "I will be back";
			group1.Child = box0;
			
			Gallery gallery = new Gallery ();
			gallery.AppendTile (new SampleTile (new Color (1, 0, 0), new Color (0, 1, 0)));
			gallery.AppendTile (new SampleTile (new Color (0.5, 0.5, 0), new Color (0, 0, 1)));
			
			group2 = new RibbonGroup ();
			group2.Label = "Gallery";
			group2.Child = gallery;
			
			HBox page0 = new HBox (false, 2);
			page0.PackStart (group0, false, false, 0);
			page0.PackStart (group1, false, false, 0);
			page0.PackStart (group2, false, false, 0);
			
			HBox page1 = new HBox (false, 2);
			
			HBox page2 = new HBox (false, 2);
			
			Label pageLabel0 = new Label ("Page 1");
			pageLabel1 = new Label ("Page 2");
			Label pageLabel2 = new Label ("Page 3");
			
			Ribbons.Button shortcuts = new Ribbons.Button ("Menu");
			shortcuts.Child.ModifyFg (Gtk.StateType.Normal, new Gdk.Color(255, 255, 255));
			
			ribbon = new Ribbon ();
			ribbon.Shortcuts = shortcuts;
			ribbon.AppendPage (page0, pageLabel0);
			ribbon.AppendPage (page1, pageLabel1);
			ribbon.AppendPage (page2, pageLabel2);
			pageLabel1.AddEvents ((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.PointerMotionMask));
			pageLabel1.ButtonPressEvent += delegate(object sender, ButtonPressEventArgs e)
			{
				Console.WriteLine("label1 press");
			};
			pageLabel1.EnterNotifyEvent += delegate(object sender, EnterNotifyEventArgs e)
			{
				Console.WriteLine("label1 enter");
			};
			pageLabel1.LeaveNotifyEvent += delegate(object sender, LeaveNotifyEventArgs e)
			{
				Console.WriteLine("label1 leave");
			};
			
			TextView txt = new TextView ();
			
			master.PackStart (ribbon, false, false, 0);
			master.PackStart (txt, true, true, 0);
			
			Add (master);
			
			ScreenChanged += Window_OnScreenChanged;
			Window_OnScreenChanged (this, null);
			ExposeEvent += Window_OnExpose;
			DeleteEvent += Window_OnDelete;
			
			this.Resize (200, 200);
			this.ShowAll ();
		}

		private void onClick(object Sender, EventArgs e)
		{
			Dialog d = new Dialog ("Test", this, DialogFlags.DestroyWithParent);
			d.Modal = true;
			d.AddButton ("Close", ResponseType.Close);
			d.Run ();
			d.Destroy ();
		}
		
		[GLib.ConnectBefore]
		private void Window_OnExpose(object sender, ExposeEventArgs args)
		{
			Gdk.EventExpose evnt = args.Event;
			Context cr = Gdk.CairoHelper.Create (GdkWindow);
			
			if(composeAvailable)
				cr.SetSourceRGBA (0, 0, 0, 0.3);
			else
				cr.SetSourceRGB (0.3, 0.3, 0.3);
			
			cr.Operator = Operator.Source;
			cr.Paint ();
			
			args.RetVal = false;
		}
		
		private void Window_OnScreenChanged(object Send, ScreenChangedArgs args)
		{
			Gdk.Colormap cm = Screen.RgbaColormap;
			composeAvailable = cm != null;	// FIX: Do not seem to detect compose support in all cases 
			
			if(!composeAvailable) cm = Screen.RgbColormap;
			Colormap = cm;
			
			Console.WriteLine("Compose Support: " + composeAvailable);
		}
		
		private void Window_OnDelete(object send, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}
	}
}
