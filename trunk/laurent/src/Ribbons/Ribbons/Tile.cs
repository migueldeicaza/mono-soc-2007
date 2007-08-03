using System;
using Cairo;
using Gtk;

namespace Ribbons
{
	public abstract class Tile : Widget
	{
		protected Theme theme = new Theme ();
		
		public event EventHandler Clicked;
		
		/// <summary>Default constructor.</summary>
		public Tile ()
		{
			this.SetFlag (WidgetFlags.NoWindow);
			
			this.AddEvents ((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.PointerMotionMask));
		}
		
		/// <summary>Fires the Click event.</summary>
		public void Click ()
		{
			if(Clicked != null) Clicked (this, EventArgs.Empty);
		}
		
		protected override void OnSizeRequested (ref Requisition requisition)
		{
			base.OnSizeRequested (ref requisition);
		}
		
		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
		}
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			Context cr = Gdk.CairoHelper.Create (this.GdkWindow);
			
			Cairo.Rectangle area = new Cairo.Rectangle (evnt.Area.X, evnt.Area.Y, evnt.Area.Width, evnt.Area.Height);
			cr.Rectangle (area);
			cr.Clip ();
			DrawContent (cr, area);
			
			return base.OnExposeEvent (evnt);
		}
		
		/// <summary>
		/// Draws the content of the tile.
		/// </summary>
		/// <param name="Context">Cairo context to be used to draw the content.</param>
		/// <param name="Area">Area that can be painted.</param>
		public abstract void DrawContent (Cairo.Context Context, Cairo.Rectangle Area);
		
		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			bool ret = base.OnButtonPressEvent (evnt);
			
			this.QueueDraw ();
			return ret;
		}
		
		protected override bool OnButtonReleaseEvent (Gdk.EventButton evnt)
		{
			bool ret = base.OnButtonReleaseEvent (evnt);
			
			this.QueueDraw ();
			return ret;
		}
		
		protected override bool OnEnterNotifyEvent (Gdk.EventCrossing evnt)
		{
			bool ret = base.OnEnterNotifyEvent (evnt);
			
			this.QueueDraw ();
			return ret;
		}
		
		protected override bool OnLeaveNotifyEvent (Gdk.EventCrossing evnt)
		{
			bool ret = base.OnLeaveNotifyEvent (evnt);
			
			this.QueueDraw ();
			return ret;
		}
	}
}