using System;
using System.Collections.Generic;
using System.Text;

using Mono.GetOptions;
using FieldStat;

namespace FieldStatConsole
{
    public class MonoCovOptions : Options
    {
        [Option("Do not perform coderank analysis.", "no-coderank")]
        public bool nocodeRank = false;

        [Option("Path of Generated Reports (default \"output\").", "output-dir")]
        public string outputDirectory = "output";

        [Option("Path of Application Repository to collect data over.", "app-repos")]
        public string applicationRepository = "";

        [Option("Include only specified patterns: (e.g. \"System.IO.*;System.Data.*;\").", "patterns")]
        public string filters = "";

        [Option("Path of Coverage XML Files generated by monocov (default \"Data/Coverage\").", "coverage-path")]
        public string coveragePath = "../Data";

        public override WhatToDoNext DoAbout()
        {
            base.DoAbout();
            return WhatToDoNext.AbandonProgram;
        }
    }

    static class Program
    {
        //FieldStat --coverage-path="../../../Data/Coverage Data/coverage_output" FieldStat.exe
        static void Main(string[] args)
        {
            MonoCovOptions options = new MonoCovOptions();
            options.ProcessArgs(args);

            // List of assemblies/executables or a directory containing executables.
            args = options.RemainingArguments;

            if (args.Length == 0 && options.applicationRepository == "")
            {
                options.DoHelp();
            }
            Console.WriteLine("app-repos: " + options.applicationRepository);
            Console.WriteLine("coveragePath: " + options.coveragePath);
            Console.WriteLine("filters: " + options.filters);
            Console.WriteLine("no-coderank: " + options.nocodeRank);
            Console.WriteLine("outputDir: " + options.outputDirectory);
            Console.WriteLine("args: " + string.Join(",", options.RemainingArguments));

            ParameterData data = new ParameterData();
            ExportData export = new ExportData(data.Results, options.outputDirectory);

            if (options.applicationRepository != "")
                data.LoadApplicationRepository(options.applicationRepository);
            else
                data.LoadAssemblies(args);

            if (options.filters != "")
                data.LoadFilters(options.filters.Split(';'));

            data.LoadCoverage(options.coveragePath);

            data.ComputeResults();
            export.ExportResults();
        }
    }
}