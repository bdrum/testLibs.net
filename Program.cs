using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace flow
{
  /// <summary>
  /// Here I would like to try to test parallel file reading. 
  /// The idea is to generate file with integer numbers on each line
  /// and get sum of them in parallel.  
  /// </summary>



  // TODO: split file to n(threads) byte blocks
  // NOTE: blocks should have similar sizes, but the end block 
  //       may be a smaller. Also check that borders bytes are not
  //       corrupted 
  // TODO: read the blocks and get the sum
  // TODO: benchmark it. compare with the seq. mode

  public static class PFile
  {
    public const int N = 1000000;
    public static ushort nt = 8;
    public static long BlockSize = 10;
    public const string tf = @"D:\GoogleDrive\Study\notes\Programming\dotnet\examples\flow\test.txt";

    public static async Task Main(string[] args)
    {

      // GenerateFile();
      // return;
      var s = new Stopwatch();
      s.Start();

      long l = 0;

      using (var f = new FileStream(tf, FileMode.Open, FileAccess.Read))
      {
        l = f.Length; // FIXME: I really have to get byte numbers before reading? Looks it's cost nothing.
      }

      Console.WriteLine($"Byte numbers {l}");

      BlockSize = l / 2 + l % 2;

      Console.WriteLine($"Size of block {BlockSize}");

      var tsks = new Task<long>[nt];

      for (uint i = 0; i < tsks.Length; ++i)
      {
        var task_num = i;
        tsks[i] = Task.Run(() => GetBlockSum((long)task_num * BlockSize));
      }

      Task.WaitAll(tsks.ToArray());
      Console.WriteLine($"par sum:  {tsks.Select(t => t.Result).Sum()}");
      Console.WriteLine($"Elapsed time for parallel reading is {s.Elapsed.TotalSeconds}");

      s.Restart();

      Console.WriteLine($"Sequenced sum {File.ReadAllLines(tf).Select(s => long.Parse(s)).Sum()}");

      s.Stop();
      Console.WriteLine($"Elapsed time for sequenced reading is {s.Elapsed.TotalSeconds}");
    }

    public static void GenerateFile()
    {
      File.WriteAllLines(tf, Enumerable.Range(1, N).ToList().ConvertAll(i => i.ToString()));
    }

    public static long GetBlockSum(long position)
    {
      using (var f = new FileStream(tf, FileMode.Open, FileAccess.Read))
      {
        byte[] b1 = new byte[BlockSize];
        f.Seek(position, SeekOrigin.Begin);
        f.Read(b1, 0, b1.Length);
        var tmpSArr = Encoding.Default.GetString(b1).Split("\n");
        return tmpSArr.Where(s => long.TryParse(s, out _)).Select(i => long.Parse(i)).Sum();
      }
    }

  }

}
