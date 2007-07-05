using System;
using System.IO;
using System.Runtime.InteropServices;

using Glade;
using Gtk;
using GtkSharp;

namespace Mono.Debugger.Frontend
{
	public class MdbGui
	{
		[DllImport("monodebuggerserver")]
		static extern int mono_debugger_server_static_init ();

		[DllImport("monodebuggerserver")]
		static extern int mono_debugger_server_get_pending_sigint ();
		
		
		Interpreter interpreter;
		DebuggerEngine engine;
		LineParser parser;
		
		[Widget] protected TextView sourceView;
		[Widget] protected Entry consoleIn;
		[Widget] protected TextView consoleOut;
		[Widget] protected Viewport viewportLocalVariables;
		[Widget] protected Viewport viewportCallstack;
		[Widget] protected Viewport viewportThreads;
		StringWriter consoleOutWriter = new StringWriter();
		
		LocalsPad localsPad;
		CallstackPad callstackPad;
		ThreadPad threadPad;
		
		public static void Main(string[] args)
		{
			new MdbGui(args);
		}
		
		public void DebuggerInit(string[] args)
		{
			mono_debugger_server_static_init ();
			
			bool is_interactive = true;
			
			DebuggerConfiguration config = new DebuggerConfiguration ();
			config.LoadConfiguration ();

			DebuggerOptions options = DebuggerOptions.ParseCommandLine (args);
			if (options.HasDebugFlags)
				Report.Initialize (options.DebugOutput, options.DebugFlags);
			else
				Report.Initialize ();
			
			interpreter = new Interpreter (is_interactive, config, options);
			engine = interpreter.DebuggerEngine;
			parser = new LineParser (engine);
			
			if (interpreter.Options.StartTarget) {
				interpreter.Start ();
			}
		}
		
		public void GuiInit()
		{
			// Redirect output to the TextView
			Console.SetOut(consoleOutWriter);
			Console.SetError(consoleOutWriter);
			GLib.Timeout.Add(100, UpdateConsoleOut);
			
			// Load XML file
			Glade.XML gxml = new Glade.XML("gui.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			// Default source view
			TextTag  tag   = new TextTag("currentLine");
			tag.Background = "yellow";
			sourceView.Buffer.TagTable.Add(tag);
			sourceView.Buffer.Text = "No source file";
			
			// Load pads
			callstackPad = new CallstackPad(interpreter);
			viewportCallstack.Add(callstackPad);
			threadPad = new ThreadPad(interpreter);
			viewportThreads.Add(threadPad);
			localsPad = new LocalsPad(interpreter);
			viewportLocalVariables.Add(localsPad);
			
			consoleIn.GrabFocus();
		}
		
		public MdbGui(string[] args) 
		{
			Application.Init();
			
			DebuggerInit(args);
			GuiInit();
			UpdateGUI();
			
			Application.Run();
		}
		
		/// <summary> Add any new output to the TextView </summary>
		bool UpdateConsoleOut()
		{
			if (consoleOutWriter.ToString().Length > 0) {
				consoleOut.Buffer.Text += consoleOutWriter.ToString();
				consoleOutWriter = new StringWriter();
				Console.SetOut(consoleOutWriter);
				Console.SetError(consoleOutWriter);
				// Scroll the window to the end
				TextMark endMark = consoleOut.Buffer.CreateMark(null, consoleOut.Buffer.EndIter, false);
				consoleOut.ScrollToMark(endMark, 0, false, 0, 0);
			}
			return true;
		}
		
		protected void OnMainWindow_delete_event(object o, DeleteEventArgs e) 
		{
			if (interpreter.HasCurrentProcess) {
				new KillCommand().Execute(engine);
			}
			Application.Quit();
			e.RetVal = true;
		}
		
		protected void OnToolbuttonRun_clicked(object o, EventArgs e) 
		{
			Console.WriteLine("OnToolbuttonRun_clicked");
			if (interpreter.HasTarget) {
				Console.WriteLine("Error - alredy running");
			} else {
				new RunCommand().Execute(engine);
				UpdateGUI();
			}
		}
		
		protected void OnToolbuttonStop_clicked(object o, EventArgs e) 
		{
			Console.WriteLine("OnToolbuttonStop_clicked");
			if (interpreter.HasCurrentProcess) {
				new KillCommand().Execute(engine);
				UpdateGUI();
			} else {
				Console.WriteLine("Error - nothing to stop");
			}
		}
		
		protected void OnToolbuttonStepIn_clicked(object o, EventArgs e) 
		{
			Console.WriteLine("OnToolbuttonStepIn_clicked");
			if (interpreter.HasCurrentThread) {
				new StepCommand().Execute(engine);
				UpdateGUI();
			} else {
				Console.WriteLine("Error - no current thread");
			}
		}
		
		public void OnToolbuttonStepOver_clicked(object o, EventArgs e) 
		{
			Console.WriteLine("OnToolbuttonStepOver_clicked");
			if (interpreter.HasCurrentThread) {
				new NextCommand().Execute(engine);
				UpdateGUI();
			} else {
				Console.WriteLine("Error - no current thread");
			}
		}
		
		protected void OnToolbuttonStepOut_clicked(object o, EventArgs e) 
		{
			Console.WriteLine("OnToolbuttonStepOut_clicked");
			if (interpreter.HasCurrentThread) {
				new FinishCommand().Execute(engine);
				UpdateGUI();
			} else {
				Console.WriteLine("Error - no current thread");
			}
		}
		
		/// <summary> Execute entered command </summary>
		protected void OnConsoleIn_activate(object o, EventArgs e) 
		{
			if (consoleIn.Text == "g") {
				UpdateGUI();
			} else {
				parser.Append (consoleIn.Text);
				if (parser.IsComplete ()){
					parser.Execute ();
					parser.Reset ();
					UpdateGUI();
				}
			}
			
			consoleIn.Text = String.Empty;
		}
		
		public void UpdateGUI()
		{
			Console.WriteLine("Updating GUI...");
			
			// Update pads - roughly the fastest ones first
			threadPad.UpdateDisplay();
			callstackPad.UpdateDisplay();
			localsPad.UpdateDisplay();
			
			// Update the source view
			StackFrame currentFrame = null;
			if (interpreter.HasCurrentThread) {
				currentFrame = interpreter.CurrentThread.CurrentFrame;
			}
			if (currentFrame != null &&
			    currentFrame.SourceAddress != null &&
			    currentFrame.SourceAddress.Location != null &&
			    currentFrame.SourceAddress.Location.FileName != null) {
				
				SourceBuffer buffer = interpreter.ReadFile(currentFrame.SourceAddress.Location.FileName);
				string[] sourceCode = buffer.Contents;
				sourceView.Buffer.Text = string.Join("\n", sourceCode);
				
				int line = currentFrame.SourceAddress.Location.Line;
				TextIter begin = sourceView.Buffer.GetIterAtLine(line - 1);
				TextIter end   = sourceView.Buffer.GetIterAtLine(line);
				sourceView.Buffer.ApplyTag("currentLine", begin, end);
				
				sourceView.ScrollToIter(end, 0, false, 0, 0);
			} else {
				sourceView.Buffer.Text = "No source code";
			}
		}
	}
}
