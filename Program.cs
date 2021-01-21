using System;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Linq;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using System.Text;
using System.Collections.Generic;
namespace flow
{
  public class Program
  {
    public static void Main(string[] args)
    {


      var w = new Stopwatch();
      w.Start();


      long l = 0;
      using (var fs = new FileStream(LargeFileParser.tf, FileMode.Open, FileAccess.Read))
        l = fs.Length;

      Console.WriteLine(l);
      Console.WriteLine((int)l);
      Console.WriteLine(int.MaxValue / (1024 * 1024 * 1024));

      var bts = new List<byte>(int.MaxValue - 1);

      using (var m = MemoryMappedFile.CreateFromFile(LargeFileParser.tf, FileMode.Open))
      {
        using (var v = m.CreateViewAccessor(0, l))
        {
          var s = 100000;
          for (long i = 0; i < l - 100000; i += s)
          {
            var b = new byte[s];
            v.ReadArray(i, b, 0, b.Length);
            bts.AddRange(b);
          }
        }
      }

      Console.WriteLine(Encoding.UTF8.GetString(bts.ToArray()).AsParallel().TakeLast(100).ToArray());
      Console.WriteLine($"Elapsed time: {w.Elapsed.TotalSeconds}");
      // var summary = BenchmarkRunner.Run<Sum>();

    }
  }
}

