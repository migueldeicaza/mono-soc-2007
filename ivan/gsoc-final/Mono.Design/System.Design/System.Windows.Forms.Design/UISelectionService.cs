/* vi:set ts=4 sw=4: */
//
// System.Windows.Forms.Design.UISelectionService
//
// Authors:
//	  Ivan N. Zlatev (contact i-nZ.net)
//
// (C) 2007 Ivan N. Zlatev

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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mono.Design
{
	
	
	internal class UISelectionService : SelectionService, IUISelectionService
	{

		private IServiceProvider _serviceProvider;

		public UISelectionService (IServiceProvider serviceProvider) : base (serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException ("serviceProvider");

			_serviceProvider = serviceProvider;
		}

		private object GetService (Type service)
		{
			return _serviceProvider.GetService (service);
		}

		public bool SelectionInProgress {
			get { return _selecting;  }
		}
		
		public bool DragDropInProgress {
			get { return _dragging;  }
		}

		public bool ResizeInProgress{
			get { return _resizing; }
		}
	   

		public bool SetCursor (int x, int y)
		{
			bool modified = false;
			// if moving mouse around - set cursor if mouse is hovering a selectionframes' grabhandles
			//
			SelectionFrame frame = GetSelectionFrameAt (x, y);
			if (frame != null && frame.HitTest (x, y) && frame.SetCursor (x, y))
				modified = true;

			return modified;
		}
		
		
		public void MouseDragBegin (Control container, int x, int y)
		{
			// XXX: pass ControlDesigner and not control and check if it is a ParentControlDesigner !!!!!!
			//
			// * start resizing the selection frame
			// * start selecting
			//		   
			SelectionFrame frame = GetSelectionFrameAt (x, y);
			
			if (frame != null && frame.HitTest (x, y)) {
				this.SetSelectedComponents (new IComponent[] { frame.Control });
				this.ResizeBegin (x, y);
			}
			else {
				SelectionBegin (container, x, y);
			}				
		}
		
		public void MouseDragMove (int x, int y)
		{
			if (_selecting)
				SelectionContinue (x, y);
			else if (_resizing)
				ResizeContinue (x, y);
		}
		
		public void MouseDragEnd (bool cancel)
		{
			if (_selecting)
				SelectionEnd (cancel);
			else if (_resizing)
				ResizeEnd (cancel);

			if (Cursor.Current != Cursors.Default)
				Cursor.Current = Cursors.Default;
		}


#region Dragging
		private bool _dragging = false;
		private Point _prevMousePosition;
		private bool _firstMove = false;

		// container coordinates (primary selection's)
		//
		public void DragBegin ()
		{
			_dragging = true;
			_firstMove = true;
			// TODO: Use the undo/redo mechanism to declare a state change, so that if the dragging is canceled
			// the locations and container of the controls will be restored to the one before the dragging
			//
			if (this.PrimarySelection != null)
				((Control)this.PrimarySelection).DoDragDrop (new ControlDataObject ((Control)this.PrimarySelection), DragDropEffects.All);
		}
		
		// container cordinates
		//
		public void DragOver (Control container, int x, int y)
		{
			//Console.WriteLine ("DragOver: " + x + " : " + y);
			//Console.WriteLine ("in container: " + container.ToString ());
			if (_dragging) {
				if (_firstMove) {
					_prevMousePosition = new Point (x, y);
					_firstMove = false;
				}
				else {
					int dx = x - _prevMousePosition.X;
					int dy = y - _prevMousePosition.Y;
					MoveSelection (container, dx, dy);
					_prevMousePosition = new Point (x, y);

					// Repaint everything >_<
					//
					IDesignerHost host = this.GetService (typeof (IDesignerHost)) as IDesignerHost;
					if (host != null && host.RootComponent != null)
						((Control)host.RootComponent).Refresh ();
					//container.Refresh ();
				}
			}
		}
		
		// container coordinates
		//
		public void DragDrop (bool cancel, Control container, int x, int y)
		{
			if (_dragging) {
				// TODO: Handle cancel by requesting an undo operation
				//
				int dx = x - _prevMousePosition.X;
				int dy = y - _prevMousePosition.Y;
			
				MoveSelection (container, dx, dy);
				_dragging = false;
				
				// Repaint everything
				//
				IDesignerHost host = this.GetService (typeof (IDesignerHost)) as IDesignerHost;
				if (host != null && host.RootComponent != null)
					((Control)host.RootComponent).Refresh ();
				// Send mouse up message to the primary selection
				// Else for parentcontroldesigner there is no mouseup event and it doesn't set allow drop back to false
				//
				Native.SendMessage (((Control)this.PrimarySelection).Handle, Native.Msg.WM_LBUTTONUP, (IntPtr) 0, (IntPtr) 0);
			}
		}
		
		private void MoveSelection (Control container, int dx, int dy)
		{
			bool reparent = false;
			Control oldParent = null;
			
			if (((Control)this.PrimarySelection).Parent != container && !this.GetComponentSelected (container)) {
				reparent = true;
				oldParent = ((Control)this.PrimarySelection).Parent;
			}
			
			// FIXME: Should check selectionstyle per control to determine if it's locked...
			// if locked -> don't move
			//
			ICollection selection = this.GetSelectedComponents ();
			foreach (Component component in selection) {
				Control control = component as Control;
				if (reparent) {
					control.Parent.Controls.Remove (control);
					container.Controls.Add (control);
				}

				PropertyDescriptor property = TypeDescriptor.GetProperties (control)["Location"];
				Point location = (Point) property.GetValue (control);
				location.X += dx;
				location.Y += dy;	  
				property.SetValue (control, location);
			}
			
			if (reparent) {
				oldParent.Invalidate (false);
				oldParent.Update ();
			}
		}
#endregion


#region Selection
		private bool _selecting = false;
		private Control _selectionContainer = null;
		private Point _initialMousePosition;
		private Rectangle _selectionRectangle;
		// XXX
		private ArrayList _selectionFrames = new ArrayList ();
		
		// container coordinates
		//
		private void SelectionBegin (Control container, int x, int y)
		{
			//Console.WriteLine ("SelectionBegin");
			_selecting = true;
			_selectionContainer = container;
			_prevMousePosition = new Point (x, y);
			_initialMousePosition = _prevMousePosition;
			_selectionRectangle = new Rectangle (x , y, 0, 0);
		}
		
		private void SelectionContinue (int x, int y)
		{
			//Console.WriteLine ("SelectionContinue");
			if (_selecting) {
				// right to right
				//
				if (x > _selectionRectangle.Right) {
					_selectionRectangle.Width = x - _selectionRectangle.X;
				}
				// right to left 
				else if (x > _selectionRectangle.X && x < _selectionRectangle.Right &&
					 x < _prevMousePosition.X) {
	
					_selectionRectangle.Width = x - _selectionRectangle.X;
				}
				// left to left - f
				else if (x < _selectionRectangle.X) {
					
					// hasn't flipped
					if (_prevMousePosition.X > _selectionRectangle.X) {
						_selectionRectangle.X = _initialMousePosition.X;
						_selectionRectangle.Width = 0;
					}
					else {
						_selectionRectangle.Width += _selectionRectangle.X - x;
						_selectionRectangle.X = x;
					}
				}
				// left to right - f 
				else if (x > _selectionRectangle.X && x < _selectionRectangle.Right &&
					 x > _prevMousePosition.X) {
					
					if (_prevMousePosition.X < _selectionRectangle.X) {
						_selectionRectangle.X = _initialMousePosition.X;
						_selectionRectangle.Width = 0;
					}
					else {
						_selectionRectangle.Width -= x - _selectionRectangle.X;
						_selectionRectangle.X = x;
					}
				}
				

				if (y > _selectionRectangle.Bottom) {
					_selectionRectangle.Height = y - _selectionRectangle.Y;
				}
				else if (y > _selectionRectangle.Y && y < _selectionRectangle.Bottom &&
					 y < _prevMousePosition.Y) {

					_selectionRectangle.Height = y - _selectionRectangle.Y;

				}
				else if (y < _selectionRectangle.Y) {
					if (_prevMousePosition.Y > _selectionRectangle.Y) {
						_selectionRectangle.Y = _initialMousePosition.Y;
						_selectionRectangle.Height = 0;
					}
					else {
						_selectionRectangle.Height += _selectionRectangle.Y - y;
						_selectionRectangle.Y = y;
					}
				}
				else if (y > _selectionRectangle.Y && y < _selectionRectangle.Bottom &&
					 y > _prevMousePosition.Y) {

					if (_prevMousePosition.Y < _selectionRectangle.Y) {
						_selectionRectangle.Y = _initialMousePosition.Y;
						_selectionRectangle.Height = 0;
					}
					else {
						_selectionRectangle.Height -= y - _selectionRectangle.Y;
						_selectionRectangle.Y = y;
					}
				}

				_prevMousePosition.X = x;
				_prevMousePosition.Y = y;
				
				_selectionContainer.Refresh ();
			}
		}
		
		private void SelectionEnd (bool cancel)
		{
			//Console.WriteLine ("SelectionEnd");
			_selecting = false;
			ICollection selectedControls = GetControlsIn (_selectionRectangle);
			// do not change selection if nothing has changed
			//
			if (selectedControls.Count != 0)
				this.SetSelectedComponents (selectedControls, SelectionTypes.Replace);

			_selectionContainer.Refresh ();
		}
#endregion


#region Resizing
		private SelectionFrame _selectionFrame;
		private bool _resizing = false;

		private void ResizeBegin (int x, int y)
		{
			_resizing = true;
			_selectionFrame = this.GetSelectionFrameAt (x, y);
			_selectionFrame.ResizeBegin (x, y);
		}

		private void ResizeContinue (int x, int y)
		{
			Rectangle deltaBounds = _selectionFrame.ResizeContinue (x, y);
			ICollection selection = this.GetSelectedComponents ();

			foreach (IComponent component in selection) {
				if (component is Control) {
					SelectionFrame frame = GetSelectionFrameFor ((Control)component);
					if (frame != _selectionFrame)
						frame.Resize (deltaBounds);
				}
			}

		}

		private void ResizeEnd (bool cancel)
		{
			_selectionFrame.ResizeEnd (cancel);
			_resizing = false;
		}


		private SelectionFrame GetSelectionFrameAt (int x, int y)
		{
			SelectionFrame result = null;

			foreach (SelectionFrame frame in _selectionFrames) {
				if (frame.Bounds.Contains (new Point (x, y))) {
					result = frame;
					break;
				}
			}

			return result;
		}

		private SelectionFrame GetSelectionFrameFor (Control control)
		{
			foreach (SelectionFrame frame in _selectionFrames) {
				if (control == frame.Control)
					return frame;
			}
			return null;
		}
#endregion

		public bool AdornmentsHitTest (Control control, int x, int y)
		{
			SelectionFrame frame = GetSelectionFrameAt (x, y);
			if (frame != null)
				return frame.HitTest (x, y);

			return false;
		}

		// This method is called by all ParentControlDesigner.OnPaintAdornments.
		// Selection frames are drawn in the parent container of the primary selection
		// selection rectangle is drawn in the primary selection
		//
		public void PaintAdornments (Control container, Graphics gfx)
		{
			IDesignerHost host = this.GetService (typeof (IDesignerHost)) as IDesignerHost;

			if (host == null || !(this.PrimarySelection is Control))
				 return;

			if ((Control)this.PrimarySelection == container) {
				if (_selecting) {
					Color negativeColor = Color.FromArgb ((byte)~(_selectionContainer.BackColor.R),
						 (byte)~(_selectionContainer.BackColor.G),
						 (byte)~(_selectionContainer.BackColor.B));
					DrawSelectionRectangle (gfx, _selectionRectangle, negativeColor);
				}
			}
			else if (((Control)this.PrimarySelection).Parent == container) {
				foreach (SelectionFrame frame in _selectionFrames)
					frame.OnPaint (gfx);
			}
		}


		private void DrawSelectionRectangle (Graphics gfx, Rectangle frame, Color color)
		{	   
			Pen pen = new Pen (color);
			pen.DashStyle = DashStyle.Dash;
			gfx.DrawRectangle (pen, frame);
		}
		
		
		protected override void OnSelectionChanged ()
		{

			ICollection selection = this.GetSelectedComponents ();
			
			/* XXX: uncoment when merging with design-time stuff
			if (_selectionFrames.Count == 0) {
				foreach (Component component in selection) {
					_selectionFrames.Add (new SelectionFrame ((Control) component));
				} // this code should get executed only once! (when initial primary selection is set)
			}
			else {
			*/
				int i = 0;
				foreach (Component component in selection) {
					if (i >= _selectionFrames.Count)
						_selectionFrames.Add (new SelectionFrame ((Control) component));
					else
						((SelectionFrame)_selectionFrames[i]).Control = (Control) component;
					i++;
				}
				if (i < _selectionFrames.Count)
					_selectionFrames.RemoveRange (i, _selectionFrames.Count - i);  
		   // }
			// Refresh the whole design surface (including the view)
			//
			IDesignerHost host = this.GetService (typeof (IDesignerHost)) as IDesignerHost;
			Control root = host.RootComponent as Control;
			if (root != null) {
				if (root.Parent != null)
					root.Parent.Refresh ();
				else
					root.Refresh ();
			}
			
			base.OnSelectionChanged ();
		}
		
		private ICollection GetControlsIn (Rectangle rect)
		{   
			ArrayList selectedControls = new ArrayList ();
			
			foreach (Control control in _selectionContainer.Controls) {
				if (rect.Contains (control.Bounds) || rect.IntersectsWith (control.Bounds))
					selectedControls.Add (control);
			}
			return selectedControls;
		}

	}
}
