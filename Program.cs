using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using BenchmarkDotNet.Running;
using CsvHelper;


namespace flow
{

    public class Model
    {
        public ulong entry { get; set; } // 0
        public ulong subentry { get; set; } // 0
        public float T_NumberOfSigmaTPCPion { get; set; } // -0.9801107
        public float T_Eta { get; set; } // 0.092463255
        public float T_Phi { get; set; } // 3.91371
        public float T_Px { get; set; } // -0.5406657
        public float T_Py { get; set; } // -0.5264923
        public float T_Pz { get; set; } // 0.0698779
        public short T_Q { get; set; } // 1
        public bool T_HasPointOnITSLayer0 { get; set; } // TRUE
        public bool T_HasPointOnITSLayer1 { get; set; } // FALSE
        public long T_ITSModuleInner { get; set; } // 49130500
        public long T_ITSModuleOuter { get; set; } // 177540103
        public uint T_TPCNCls { get; set; } // 157
        public bool T_TPCRefit { get; set; } // TRUE

    }


    public class Program
    {
        public static void Main(string[] args)
        {


            IA a = new A();
            a.SomeCallA();

            IB b = new A();
            b.SomeCallB();

            var c = new A();
            c.SomeCallA();
            c.SomeCallB();


            //var summary = BenchmarkRunner.Run<Sum>();


            //var s = new Sum();

            var w = new Stopwatch();
                  w.Start();


            var f = new CSVReader();

            //Console.WriteLine(f.Content.ToString());

            IEnumerable<Model> records;

            using (var reader = new StringReader(f.Content.ToString()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.IgnoreBlankLines = true;
                csv.Configuration.IgnoreQuotes = true;
                csv.Configuration.IgnoreReferences = true;
                records = csv.GetRecords<Model>();


                Console.WriteLine(records.Count());

            }


            //var l1 = Sum.SplitFileToBlocks();

            //foreach (var ll in l1)
            //{
            //    Console.WriteLine(string.Join(' ', ll.Select(ss => $"{ss:x2}")));
            //    Console.WriteLine();
            //}


            //Console.WriteLine();



            //var l = Sum.GetAlignedBlock();

            //foreach (var ll in l)
            //{
            //    Console.WriteLine(string.Join(' ', ll.Select( ss => $"{ss:x2}")));
            //    Console.WriteLine();
            //    Console.WriteLine(System.Text.Encoding.UTF8.GetString(ll));
            //}

            Console.WriteLine($"ParSum elapsed time: {w.Elapsed.TotalSeconds}");
                  w.Stop();
        }
    }
}

