using Cairo;
using System;

namespace Ribbons
{
	public class Theme
	{
		internal enum ButtonState
		{
			Default, Hover, Pressed
		}
		
		protected ColorScheme colorScheme = new ColorScheme ();
		
		public void DrawGroup (Context cr, Rectangle r, double roundSize, double lineWidth, double space, Pango.Layout l, RibbonGroup w)
		{
			double lineWidth05 = lineWidth/2, lineWidth15 = 3*lineWidth05;
			LinearGradient linGrad;
			
			double x0 = r.X + roundSize, x1 = r.X + r.Width - roundSize;
			double y0 = r.Y + roundSize, y1 = r.Y + r.Height - roundSize;
			cr.Arc (x1, y1, roundSize - lineWidth05, 0, Math.PI/2);
			cr.Arc (x0 + lineWidth, y1, roundSize - lineWidth05, Math.PI/2, Math.PI);
			cr.Arc (x0, y0, roundSize - lineWidth15, Math.PI, 3*Math.PI/2);
			cr.Arc (x1, y0 + lineWidth, roundSize - lineWidth05, 3*Math.PI/2, 0);
			cr.LineTo (x1 + roundSize - lineWidth05, y1);
			cr.LineWidth = lineWidth;
			cr.Color = colorScheme.Bright;
			cr.Stroke ();
			
			if(l != null)
			{
				int lblWidth, lblHeight;
				Pango.CairoHelper.UpdateLayout (cr, l);
				l.GetPixelSize(out lblWidth, out lblHeight);
				
				double bandHeight = lblHeight + 2*space;
				cr.Arc (x1, y1, roundSize - lineWidth15, 0, Math.PI/2);
				cr.Arc (x0, y1 - lineWidth, roundSize - lineWidth05, Math.PI/2, Math.PI);
				double bandY = y1 + roundSize - 2*lineWidth - bandHeight;
				cr.LineTo (x0 - roundSize + lineWidth05, bandY);
				cr.LineTo (x1 + roundSize - lineWidth15, bandY);
				linGrad = new LinearGradient (0, bandY, 0, bandY + bandHeight);
				linGrad.AddColorStop (0.0, colorScheme.Dark);
				linGrad.AddColorStop (1.0, colorScheme.PrettyDark);
				cr.Pattern = linGrad;
				cr.Fill ();
				
				cr.Save ();
				cr.Rectangle (r.X + 2*lineWidth, bandY, r.Width - 4*lineWidth, bandHeight);
				cr.Clip ();
				
				cr.Color = new Color(1, 1, 1);
				Pango.CairoHelper.UpdateLayout (cr, l);
				cr.MoveTo (r.X + Math.Max(2*lineWidth, (r.Width - lblWidth) / 2), bandY + space);
				Pango.CairoHelper.ShowLayout (cr, l);
				
				cr.Restore();
			}
			
			cr.MoveTo (x1 + roundSize - lineWidth15, y1);
			cr.Arc (x1, y1, roundSize - lineWidth15, 0, Math.PI/2);
			cr.Arc (x0, y1 - lineWidth, roundSize - lineWidth05, Math.PI/2, Math.PI);
			cr.Arc (x0, y0, roundSize - lineWidth05, Math.PI, 3*Math.PI/2);
			cr.Arc (x1 - lineWidth, y0, roundSize - lineWidth05, 3*Math.PI/2, 0);
			cr.LineTo (x1 + roundSize - lineWidth15, y1);
			cr.LineWidth = lineWidth;
			linGrad = new LinearGradient (0, r.Y, 0, r.Y + r.Height - lineWidth);
			linGrad.AddColorStop (0.0, ColorScheme.GetColorRelative (colorScheme.PrettyDark, 0.1));
			linGrad.AddColorStop (1.0, colorScheme.PrettyDark);
			cr.Pattern = linGrad;
			cr.Stroke ();
		}
		
		public Gdk.Color GetForecolorForRibbonTabs (bool Selected)
		{
			if(Selected)
				return new Gdk.Color (0, 0, 0);
			else
				return new Gdk.Color (255, 255, 255);
		}
		
		public void DrawRibbon (Context cr, Gdk.Rectangle bodyAllocation, double roundSize, double lineWidth, Ribbon widget)
		{
			double lineWidth05 = lineWidth / 2;
			double lineWidth15 = 3 * lineWidth05;
			double x0, x1, y0, y1;
			LinearGradient linGrad;
			
			Ribbon.RibbonPage p = widget.CurrentPage;
			if(p != null)
			{
				//Color c = ColorScheme.GetColor(colorScheme.Bright, 0.92);
				Color c = colorScheme.Normal;
				
				if(bodyAllocation.Height > 0)
				{
					/*** PAGE ***/
					
					x0 = bodyAllocation.X; x1 = bodyAllocation.X + bodyAllocation.Width;
					y0 = bodyAllocation.Y; y1 = bodyAllocation.Y + bodyAllocation.Height;
					
					cr.Arc (x0 + roundSize, y1 - roundSize, roundSize - lineWidth05, Math.PI/2, Math.PI);
					cr.Arc (x0 + roundSize, y0 + roundSize, roundSize - lineWidth05, Math.PI, 3*Math.PI/2);
					cr.Arc (x1 - roundSize, y0 + roundSize, roundSize - lineWidth05, 3*Math.PI/2, 0);
					cr.Arc (x1 - roundSize, y1 - roundSize, roundSize - lineWidth05, 0, Math.PI/2);
					cr.LineTo (x0 + roundSize, y1 - lineWidth05);
					
					/*** BACKGOUND ***/
					cr.Color = c;
					cr.FillPreserve ();

					/*** DARK BORDER ***/
					cr.LineWidth = lineWidth;
					cr.Color = ColorScheme.GetColorAbsolute (colorScheme.Normal, 0.75);
					cr.Stroke ();
					
					/*** GLASS EFFECT ***/
					double ymid = Math.Round(y0 + (y1 - y0) * 0.25);
					
					cr.Arc (x0 + roundSize, y0 + roundSize, roundSize - lineWidth, Math.PI, 3*Math.PI/2);
					cr.Arc (x1 - roundSize, y0 + roundSize, roundSize - lineWidth, 3*Math.PI/2, 0);
					cr.LineTo (x1 - lineWidth, ymid);
					cr.LineTo (x0 + lineWidth, ymid);
					cr.LineTo (x0 + lineWidth, y0 + roundSize);
					linGrad = new LinearGradient (0, y0, 0, ymid);
					linGrad.AddColorStop (0.0, new Color (0, 0, 0, 0.0));
					linGrad.AddColorStop (1.0, new Color (0, 0, 0, 0.075));
					cr.Pattern = linGrad;
					cr.Fill ();
					
					cr.Arc (x0 + roundSize, y1 - roundSize, roundSize - lineWidth, Math.PI/2, Math.PI);
					cr.LineTo (x0 + lineWidth, ymid);
					cr.LineTo (x1 - lineWidth, ymid);
					cr.Arc (x1 - roundSize, y1 - roundSize, roundSize - lineWidth, 0, Math.PI/2);
					cr.LineTo (x0 + roundSize, y1 - lineWidth);
					linGrad = new LinearGradient (0, ymid, 0, y1);
					linGrad.AddColorStop (0.0, new Color (0, 0, 0, 0.1));
					linGrad.AddColorStop (0.5, new Color (0, 0, 0, 0.0));
					cr.Pattern = linGrad;
					cr.Fill ();
				}
				
				/*** TAB ***/
				
				Gdk.Rectangle r = p.LabelAllocation;
				
				x0 = r.X; x1 = r.X + r.Width;
				y0 = r.Y; y1 = r.Y + r.Height + lineWidth;
				
				/*** TAB :: BACKGROUND ***/
				
				cr.MoveTo (x0 + lineWidth05, y1);
				cr.LineTo (x0 + lineWidth05, y0 + roundSize);
				cr.Arc (x0 + roundSize, y0 + roundSize, roundSize - lineWidth05, Math.PI, 3*Math.PI/2);
				cr.Arc (x1 - roundSize, y0 + roundSize, roundSize - lineWidth05, 3*Math.PI/2, 0);
				cr.LineTo (x1 - lineWidth05, y1);
				
				linGrad = new LinearGradient (0, y0, 0, y1);
				linGrad.AddColorStop (0.0, colorScheme.PrettyBright);
				linGrad.AddColorStop (1.0, c);
				cr.Pattern = linGrad;
				cr.Fill ();
				
				/*** TAB :: DARK BORDER ***/
				
				cr.MoveTo (x0 + lineWidth05, y1);
				cr.LineTo (x0 + lineWidth05, y0 + roundSize);
				cr.Arc (x0 + roundSize, y0 + roundSize, roundSize - lineWidth05, Math.PI, 3*Math.PI/2);
				cr.Arc (x1 - roundSize, y0 + roundSize, roundSize - lineWidth05, 3*Math.PI/2, 0);
				cr.LineTo (x1 - lineWidth05, y1);
				
				cr.LineWidth = lineWidth;
				cr.Color = ColorScheme.GetColorRelative (colorScheme.Bright, -0.1);
				cr.Stroke ();
				
				y1 -= 1.0;
				
				/*** TAB :: HIGHLIGHT ***/
				
				cr.MoveTo (x0 + lineWidth15, y1);
				cr.LineTo (x0 + lineWidth15, y0 + roundSize);
				cr.Arc (x0 + roundSize, y0 + roundSize, roundSize - lineWidth15, Math.PI, 3*Math.PI/2);
				cr.Arc (x1 - roundSize, y0 + roundSize, roundSize - lineWidth15, 3*Math.PI/2, 0);
				cr.LineTo (x1 - lineWidth15, y1);
				
				cr.LineWidth = lineWidth;
				linGrad = new LinearGradient (0, y0+lineWidth, 0, y1);
				linGrad.AddColorStop (0.0, colorScheme.PrettyBright);
				linGrad.AddColorStop (1.0, ColorScheme.SetAlphaChannel (colorScheme.Bright, 0));
				cr.Pattern = linGrad;
				cr.Stroke ();
				
				/*** TAB :: SHADOW ***/
				
				cr.MoveTo (x0 - lineWidth05, y1);
				cr.LineTo (x0 - lineWidth05, y0 + roundSize);
				cr.Arc (x0 + roundSize, y0 + roundSize, roundSize + lineWidth05, Math.PI, 3*Math.PI/2);
				cr.Arc (x1 - roundSize, y0 + roundSize, roundSize + lineWidth05, 3*Math.PI/2, 0);
				cr.LineTo (x1 + lineWidth05, y1);
				
				cr.LineWidth = lineWidth;
				cr.Color = new Color (0, 0, 0, 0.2);
				cr.Stroke ();
			}
		}
		
		internal void DrawButton (Context cr, Rectangle bodyAllocation, ButtonState state, double roundSize, double lineWidth, Button widget)
		{
			if(state == ButtonState.Default) return;
			
			double lineWidth05 = lineWidth / 2;
			double lineWidth15 = lineWidth05 * 3;
			
			LinearGradient bodyPattern, innerBorderPattern;
			Color borderColor;
			
			if(state == ButtonState.Pressed)
			{
				bodyPattern = new LinearGradient (bodyAllocation.X, bodyAllocation.Y, bodyAllocation.X, bodyAllocation.Y + bodyAllocation.Height);
				bodyPattern.AddColorStopRgb (0.0, new Color (0.996, 0.847, 0.667));
				bodyPattern.AddColorStopRgb (0.4, new Color (0.984, 0.710, 0.396));
				bodyPattern.AddColorStopRgb (0.4, new Color (0.980, 0.616, 0.204));
				bodyPattern.AddColorStopRgb (1.0, new Color (0.992, 0.933, 0.667));
				
				innerBorderPattern = new LinearGradient (bodyAllocation.X, bodyAllocation.Y, bodyAllocation.X, bodyAllocation.Y + bodyAllocation.Height);
				innerBorderPattern.AddColorStop (0.0, new Color (0.876, 0.718, 0.533, 1));
				innerBorderPattern.AddColorStop (1.0, new Color (0, 0, 0, 0));
				
				borderColor = new Color (0.824, 0.753, 0.553);
			}
			else
			{
				bodyPattern = new LinearGradient (bodyAllocation.X, bodyAllocation.Y, bodyAllocation.X, bodyAllocation.Y + bodyAllocation.Height);
				bodyPattern.AddColorStopRgb (0.0, new Color (1, 0.996, 0.890));
				bodyPattern.AddColorStopRgb (0.4, new Color (1, 0.906, 0.592));
				bodyPattern.AddColorStopRgb (0.4, new Color (1, 0.843, 0.314));
				bodyPattern.AddColorStopRgb (1.0, new Color (1, 0.906, 0.588));
				
				innerBorderPattern = new LinearGradient (bodyAllocation.X, bodyAllocation.Y, bodyAllocation.X, bodyAllocation.Y + bodyAllocation.Height);
				innerBorderPattern.AddColorStop (0.0, new Color (1, 1, 0.969, 1));
				innerBorderPattern.AddColorStop (1.0, new Color (0, 0, 0, 0));
				
				borderColor = new Color (0.671, 0.631, 0.549);
			}
			
			cr.LineWidth = lineWidth;
			
			double x0 = bodyAllocation.X + lineWidth15, y0 = bodyAllocation.Y + lineWidth15;
			double x1 = bodyAllocation.X + bodyAllocation.Width - lineWidth15, y1 = bodyAllocation.Y + bodyAllocation.Height - lineWidth15;
			
			cr.MoveTo (x0, y0);
			cr.LineTo (x1, y0);
			cr.LineTo (x1, y1);
			cr.LineTo (x0, y1);
			cr.LineTo (x0, y0);
			
			cr.Pattern = bodyPattern;
			cr.FillPreserve ();
			
			cr.Pattern = innerBorderPattern;
			cr.Stroke ();
			
			x0 = bodyAllocation.X + lineWidth05; y0 = bodyAllocation.Y + lineWidth05;
			x1 = bodyAllocation.X + bodyAllocation.Width - lineWidth05; y1 = bodyAllocation.Y + bodyAllocation.Height - lineWidth05;
			
			cr.MoveTo (x0, y0);
			cr.LineTo (x1, y0);
			cr.LineTo (x1, y1);
			cr.LineTo (x0, y1);
			cr.LineTo (x0, y0);
			
			cr.Color = borderColor;
			cr.Stroke ();
		}
		
		
		private void BottomRoundRectanglePath (Context cr, Rectangle r, double radius)
		{
			cr.Arc (r.X + r.Width - radius, r.Y + r.Height - radius, radius, 0, Math.PI/2);
			cr.Arc (r.X + radius, r.Y + r.Height - radius, radius, Math.PI/2, Math.PI);
			cr.LineTo (r.X, r.Y);
			cr.LineTo (r.X + r.Width, r.Y);
		}
		
		private void RoundRectanglePath (Context cr, Rectangle r, double radius)
		{
			cr.Arc (r.X + r.Width - radius, r.Y + r.Height - radius, radius, 0, Math.PI/2);
			cr.Arc (r.X + radius, r.Y + r.Height - radius, radius, Math.PI/2, Math.PI);
			cr.Arc (r.X + radius, r.Y + radius, radius, Math.PI, 3*Math.PI/2);
			cr.Arc (r.X + r.Width - radius, r.Y + radius, radius, 3*Math.PI/2, 0);
			cr.LineTo (r.X + r.Width, r.Y + r.Height - radius);
		}
	}
}