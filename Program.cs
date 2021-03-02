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
      Console.WriteLine($"Run1 is starting:");
      Run1();
      w.Stop();
      Console.WriteLine($"Elapsed time: {w.Elapsed.TotalSeconds}");
      w.Restart();
      Console.WriteLine($"Run2 is starting:");
      Run2();
      Console.WriteLine($"Elapsed time: {w.Elapsed.TotalSeconds}");

      // var summary = BenchmarkRunner.Run<Sum>();

    }

    public static int _cnt = 300;
    public static int _rn = 2;

    public static void Run1()
    {
      var cl = new CircleList<int>(Enumerable.Range(0, _cnt));

      // int roundNum = 0;
      // foreach (var i in cl)
      // {
      //   // Console.WriteLine(i);
      //   // Thread.Sleep(1000);
      //   if (i == cl.Last())
      //   {
      //     roundNum++;
      //     // Console.WriteLine($"Round {roundNum}");
      //   }
      //   if (roundNum == 10)
      //   {
      //     Console.WriteLine("It was the final round");
      //     break;
      //   }
      // }

      // Console.WriteLine("Go forward:");

      for (var i = 0; i < _rn * _cnt; ++i)
      {
        cl.MoveNext();
        // Console.WriteLine(cl.CurrentItem);
        // Thread.Sleep(1000);
      }

      // Console.WriteLine("Go back:");

      // for (var i = 0; i < 5; ++i)
      // {
      //   cl.MovePrev();
      //   Console.WriteLine(cl.CurrentItem);
      //   Thread.Sleep(1000);
      // }

    }

    public static void Run2()
    {
      var cl = new Regata.Core.Collections.CircularList<int>(Enumerable.Range(0, _cnt));
      for (var i = 0; i < _rn * _cnt; ++i)
      {
        var o = cl.NextItem;
        // Console.WriteLine(cl.NextItem);
        // Thread.Sleep(1000);
      }

    }
  }
}

