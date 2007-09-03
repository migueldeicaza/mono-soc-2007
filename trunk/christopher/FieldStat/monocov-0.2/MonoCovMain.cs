//
// TODO:
// - the line number information generated for some methods is funky, like
//   System.Text.Encoding::get_ASCII
// - the line number information for System.CurrentTimeZone:GetDaylightChanges
//   contains a 0->196 mapping !!!
// - the line number information for life.cs does not contain anything for line
//   38.
// - when a method signature consists of multiple lines, the begin_line for
//   the methods seems to be the last line of the signature, instead of the 
//   first.
// - the line number information does not contain columns, making it
//   impossible to determine coverage for code like this:
//     if (something) { something }
// - why doesn't some line numbers do not appear in stack traces ????

using System;
using System.Reflection;
using System.IO;
using System.Text;
using Mono.GetOptions;
#if GUI_qt
using MonoCov.Gui.Qt;
#else
using MonoCov.Gui.Gtk;
#endif

[assembly: AssemblyTitle("monocov")]
[assembly: AssemblyDescription("A Coverage Analysis program for .NET")]
[assembly: AssemblyCopyright("Copyright (C) 2003 Zoltan Varga")]
[assembly: Mono.Author("Zoltan Varga (vargaz@freemail.hu)")]
[assembly: AssemblyVersion(Constants.Version)]
[assembly: Mono.UsageComplement("[<datafile>]")]
[assembly: Mono.About("")]

namespace MonoCov {

public class MonoCovOptions : Options
{
	[Option("Export coverage data as XML into directory PARAM", "export-xml")]
		public string exportXmlDir;

	[Option("Export coverage data as HTML into directory PARAM", "export-html")]
		public string exportHtmlDir;

	[Option("Export coverage data for FieldStat into directory PARAM","export-fieldstat")]
		public string exportFieldStatDir;

	[Option("Use the XSL stylesheet PARAM for XML->HTML conversion", "stylesheet")]
		public string styleSheet;

	[Option("No progress messages during the export process", "no-progress")]
		public bool quiet = false;

	public override WhatToDoNext DoAbout() {
		base.DoAbout ();
		return WhatToDoNext.AbandonProgram;
	}
}

//
// This class should be named MonoCov, but the GUI class is already
// named that way, and it is hard to rename things in CVS...
//

public class MonoCovMain {

	public static int Main (String[] args) {
		MonoCovOptions options = new MonoCovOptions ();
		options.ProcessArgs (args);
		args = options.RemainingArguments;


		if (options.exportXmlDir != null)
			return handleExportXml (options, args);

		if (options.exportHtmlDir != null)
			return handleExportHtml (options, args);

		if (options.exportFieldStatDir != null )
			return handleExportFieldStat(options, args);
		#if GUI_qt
		return MonoCov.Gui.Qt.MonoCov.GuiMain (args);
		#else
		return MonoCov.Gui.Gtk.MonoCovGui.GuiMain (args);
		#endif
		//return 0;
	}

	private static void progressListener (object sender, XmlExporter.ProgressEventArgs e) {
		Console.Write ("\rExporting Data: " + (e.pos * 100 / e.itemCount) + "%");
	}
	private static int handleExportFieldStat(MonoCovOptions opts, string[] args) {
		if (args.Length == 0) {
			Console.WriteLine ("Error: Datafile name is required when using --export-fieldstat");
			return 1;
		}

		if (!Directory.Exists (opts.exportFieldStatDir)) {
			try {
				Directory.CreateDirectory (opts.exportFieldStatDir);
			}
			catch (Exception ex) {
				Console.WriteLine ("Error: Destination directory '" + opts.exportFieldStatDir + "' does not exist and could not be created: " + ex);
				return 1;
			}
		}
		
		CoverageModel model = new CoverageModel ();
		model.ReadFromFile (args [0]);
		FieldStatExporter exporter = new FieldStatExporter();
		exporter.DestinationDir = opts.exportFieldStatDir;
		exporter.StyleSheet = opts.styleSheet;
		exporter.Export (model);
		if (!opts.quiet) {
			Console.WriteLine ();
			Console.WriteLine ("Done.");
		}
		return 0;
	}

	private static int handleExportXml (MonoCovOptions opts, string[] args) {
		if (args.Length == 0) {
			Console.WriteLine ("Error: Datafile name is required when using --export-xml");
			return 1;
		}

		if (!Directory.Exists (opts.exportXmlDir)) {
			try {
				Directory.CreateDirectory (opts.exportXmlDir);
			}
			catch (Exception ex) {
				Console.WriteLine ("Error: Destination directory '" + opts.exportXmlDir + "' does not exist and could not be created: " + ex);
				return 1;
			}
		}
		
		CoverageModel model = new CoverageModel ();
		model.ReadFromFile (args [0]);
		XmlExporter exporter = new XmlExporter ();
		exporter.DestinationDir = opts.exportXmlDir;
		exporter.StyleSheet = opts.styleSheet;
		if (!opts.quiet)
			exporter.Progress += new XmlExporter.ProgressEventHandler (progressListener);
		exporter.Export (model);
		if (!opts.quiet) {
			Console.WriteLine ();
			Console.WriteLine ("Done.");
		}
		return 0;
	}

	private static void htmlProgressListener (object sender, HtmlExporter.ProgressEventArgs e) {
		Console.Write ("\rExporting Data: " + (e.pos * 100 / e.itemCount) + "%");
	}

	private static int handleExportHtml (MonoCovOptions opts, string[] args) {
		if (args.Length == 0) {
			Console.WriteLine ("Error: Datafile name is required when using --export-html");
			return 1;
		}

		if (!Directory.Exists (opts.exportHtmlDir)) {
			try {
				Directory.CreateDirectory (opts.exportHtmlDir);
			}
			catch (Exception ex) {
				Console.WriteLine ("Error: Destination directory '" + opts.exportHtmlDir + "' does not exist and could not be created: " + ex);
				return 1;
			}
		}
		
		CoverageModel model = new CoverageModel ();
		model.ReadFromFile (args [0]);
		HtmlExporter exporter = new HtmlExporter ();
		exporter.DestinationDir = opts.exportHtmlDir;
		exporter.StyleSheet = opts.styleSheet;
		if (!opts.quiet)
			exporter.Progress += new HtmlExporter.ProgressEventHandler (htmlProgressListener);
		exporter.Export (model);
		if (!opts.quiet) {
			Console.WriteLine ();
			Console.WriteLine ("Done.");
		}
		return 0;
	}
}
}
