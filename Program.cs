using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace flow
{
    class Options
    {
        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option("stdin", Default = false, HelpText = "Read from stdin")]
        public bool stdin { get; set; }

        [Option("file", HelpText = "File name")]
        public string FileName { get; set; }

        [Value(0, MetaName = "offset", HelpText = "File offset.")]
        public long? Offset { get; set; }
    }
    class Program
    {
        static ParserResult<Options> parserResult;

        static void Main(string[] args)
        {
            parserResult = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(errs => HandleErrors(errs));
        }

        static void Run(Options options)
        {
            Console.WriteLine("parser SUCCESS");
            if (!validate(options))
            {
                Console.WriteLine("Validation fail");
                var helpText = GetHelp<Options>(parserResult);
                Console.WriteLine(helpText);
            }
        }

        static string GetHelp<T>(ParserResult<T> result)
        {
            return HelpText.AutoBuild(result, h => h, e => e);
        }

        //validate options
        static bool validate(Options options)
        {
            // do validation 
            if (options.FileName == null)
                return false;
            return true;
        }

        static void HandleErrors(IEnumerable<Error> errs)
        {
            Console.WriteLine("Parser Fail");
        }
    }

}
