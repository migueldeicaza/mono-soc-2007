/* vi:set ts=4 sw=4: */
//
// System.Windows.Forms.Design.Native
//
// Authors:
//	  Ivan N. Zlatev (contact i-nZ.net)
//
// (C) 2006-2007 Ivan N. Zlatev
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

// DESRIPTION: On Mono uses reflection to invoke XplatUI methods (it is an
// internal class for the System.Windows.Forms assembly. On MS.NET ends up in
// the native Win32 API.
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;


namespace Mono.Design
{

	internal class Native 
	{

		private static Type _xplatuiType;
		private static bool _onMSNET;
		
		static Native ()
		{
			_onMSNET = false;
			Assembly assembly = Assembly.Load ("System.Windows.Forms, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			if (assembly == null)
				throw new InvalidOperationException ("Can't load System.Windows.Forms assembly.");
			
			_xplatuiType = assembly.GetType ("System.Windows.Forms.XplatUI");
			if (_xplatuiType == null)
				_onMSNET = true;
		}

		private static object InvokeMethod (string methodName, object[] args)
		{
			MethodInfo method = _xplatuiType.GetMethod (methodName, BindingFlags.NonPublic | BindingFlags.Static | 
									BindingFlags.InvokeMethod);
			if (method == null)
				throw new InvalidOperationException (methodName + " not found!");

			return method.Invoke (null, args);
		}

		public static void DefWndProc (ref Message m)
		{   
			if (_onMSNET) {
				m.Result = DefWindowProc (m.HWnd, m.Msg, m.WParam, m.LParam);
			}
			else {
				object[] args = new object[] { m };
				m.Result = (IntPtr) InvokeMethod ("DefWndProc", args);
				m = (Message) args[0];
			}
		}

		public static IntPtr SendMessage (IntPtr hwnd, Msg message, IntPtr wParam, IntPtr lParam)
		{
			if (_onMSNET)
				return SendMessage_Win32 (hwnd, (int) message, wParam, lParam);
			else
				return (IntPtr) InvokeMethod ("SendMessage", new object[] { hwnd, message, wParam, lParam });
		}

		public static Point PointToClient (Control control, Point point)
		{
			if (control == null)
				throw new ArgumentNullException ("control");

			if (_onMSNET) {
				POINT pt = new POINT (point.X, point.Y);
				ScreenToClient (control.Handle, ref pt);
				return new Point (pt.X, pt.Y);
			}
			else {
				object[] args = new object[] { control.Handle, point.X, point.Y };
				InvokeMethod ("ScreenToClient", args);
				return new Point ((int) args[1], (int) args[2]);
			}
		}

		public static IntPtr SetParent (IntPtr childHandle, IntPtr parentHandle)
		{
			if (_onMSNET)
				return SetParent_Win32 (childHandle, parentHandle);
			else
				return (IntPtr) InvokeMethod ("SetParent", new object[] { childHandle, parentHandle });
		}
		
		
#region Helpers
		public static int HiWord (int dword)
		{
			// 12345678 -> 12340000 -> 00001234
			return ((dword >> 16) & 0x0000ffff);
		}

		public static int LoWord (int dword)
		{
			// 12345678 -> 00005678
			return (dword & 0x0000ffff);
		}
		
		public static IntPtr LParam (int hiword, int loword)
		{
			// results [hiword|loword] dword
			//
			return (IntPtr)((loword << 16) | (hiword & 0x0000FFFF));
		}
#endregion
		
		public enum Msg {
			WM_CREATE				 = 0x0001,
			WM_SETFOCUS			   = 0x0007,
			WM_PAINT				  = 0X000F,
			WM_CANCELMODE			 = 0x001F,
			WM_SETCURSOR			  = 0x0020,
			WM_CONTEXTMENU			= 0x007B,
			WM_NCHITTEST			  = 0x0084,
			//
			// AccessabilityObject
			//
			WM_GETOBJECT			  = 0x003D,
			//
			// Mouse input - Client area
			//
			WM_MOUSEFIRST			 = 0x0200,
			WM_MOUSEMOVE			  = 0x0200,
			WM_LBUTTONDOWN			= 0x0201,
			WM_LBUTTONUP			  = 0x0202,
			WM_LBUTTONDBLCLK		  = 0x0203,
			WM_RBUTTONDOWN			= 0x0204,
			WM_RBUTTONUP			  = 0x0205,
			WM_RBUTTONDBLCLK		  = 0x0206,
			WM_MBUTTONDOWN			= 0x0207,
			WM_MBUTTONUP			  = 0x0208,
			WM_MBUTTONDBLCLK		  = 0x0209,
			WM_MOUSEWHEEL			 = 0x020A,
			WM_MOUSELAST			  = 0x020A,
			WM_NCMOUSEHOVER		   = 0x02A0,
			WM_MOUSEHOVER			 = 0x02A1,
			WM_NCMOUSELEAVE		   = 0x02A2,
			WM_MOUSELEAVE			 = 0x02A3,
			//
			// Mouse input - Non-client area
			//
			WM_NCMOUSEMOVE			= 0x00A0,
			WM_NCLBUTTONDOWN		  = 0x00A1,
			WM_NCLBUTTONUP			= 0x00A2,
			WM_NCLBUTTONDBLCLK		= 0x00A3,
			WM_NCRBUTTONDOWN		  = 0x00A4,
			WM_NCRBUTTONUP			= 0x00A5,
			WM_NCRBUTTONDBLCLK		= 0x00A6,
			WM_NCMBUTTONDOWN		  = 0x00A7,
			WM_NCMBUTTONUP			= 0x00A8,
			WM_NCMBUTTONDBLCLK		= 0x00A9,
			//
			// Keyboard input
			//
			WM_KEYFIRST	   = 0x0100,
			WM_KEYDOWN		= 0x0100,
			WM_KEYUP		  = 0x0101,
			WM_CHAR		   = 0x0102,
			WM_DEADCHAR	   = 0x0103,
			WM_SYSKEYDOWN	 = 0x0104,
			WM_SYSKEYUP	   = 0x0105,
			WM_SYS1CHAR	   = 0x0106,
			WM_SYSDEADCHAR	= 0x0107,
			WM_KEYLAST		= 0x0108,
			//
			// Scrolling
			//
			WM_HSCROLL		= 0x0114,
			WM_VSCROLL		= 0x0115,

			//
			// IME - International Text
			//
			WM_IME_SETCONTEXT		 = 0x0281,
			WM_IME_NOTIFY			 = 0x0282,
			WM_IME_CONTROL			= 0x0283,
			WM_IME_COMPOSITIONFULL	= 0x0284,
			WM_IME_SELECT			 = 0x0285,
			WM_IME_CHAR			   = 0x0286,
			WM_IME_REQUEST			= 0x0288,
			WM_IME_KEYDOWN			= 0x0290,
			WM_IME_KEYUP			  = 0x0291,
			
			// MWF Custom msgs
			//
			WM_MOUSE_ENTER			= 0x0401,
		}

#region Win32 Native Method Signatures
		[StructLayout(LayoutKind.Sequential)]
		private struct POINT {
			public POINT (int x, int y) {
				this.X = x;
				this.Y = y;
			}
			
			public int X;
			public int Y;
		}

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		private static extern IntPtr DefWindowProc (IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
		
		[DllImport ("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage_Win32 (IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport ("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool ScreenToClient (IntPtr hWnd, ref POINT pt);
	
		[DllImport ("user32.dll", EntryPoint = "SetParent", CharSet = CharSet.Auto)]
		private static extern IntPtr SetParent_Win32 (IntPtr childHandle, IntPtr parentHandle);
#endregion
	}
}

