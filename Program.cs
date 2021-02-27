using System;
using System.Diagnostics;
using System.Threading;
using BenchmarkDotNet.Running;
using System.Linq;
namespace flow
{
  public class Program
  {
    public static void Main(string[] args)
    {


      var w = new Stopwatch();
      w.Start();

      var cl = new CircleList<int>(Enumerable.Range(0, 3));

      foreach (var i in cl)
      {
        Console.WriteLine(i);
        Thread.Sleep(1000);
      }

      Console.WriteLine($"Elapsed time: {w.Elapsed.TotalSeconds}");
      // var summary = BenchmarkRunner.Run<Sum>();

    }
  }
}

