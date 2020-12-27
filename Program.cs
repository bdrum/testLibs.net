using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace flow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<Sum>();


                  var s = new Sum();

                  var w = new Stopwatch();
                  w.Start();
            
                  Console.WriteLine($"ParSum result: {s.ParSum()}");
                  Console.WriteLine($"ParSum elapsed time: {w.Elapsed.TotalSeconds}");
                  w.Restart();
            
                  Console.WriteLine($"ParSumSelfImpl result: {s.ParSumSelfImpl()}");
                  Console.WriteLine($"ParSumSelfImpl elapsed time: {w.Elapsed.TotalSeconds}");
                  w.Restart();
            
                  Console.WriteLine($"SeqSum result: {s.SeqSum()}");
                  Console.WriteLine($"SeqSum elapsed time: {w.Elapsed.TotalSeconds}");
                  w.Stop();
        }
    }
}

