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


  // TODO: see the disk usage plot
  // TODO: is it possible to read one block of disk simultaneously? No, it physically impossible.



  // NOTE: there is interesting idea that if hdd has only sequence access to the bits, in case of parallel tasks of reading
  //       hdd will need to waste additional time to seeking new blocks and reading the founded. This means parallel reading 
  //       should be slower then sequenced. 

  // NOTE: (To fix this I have to save long as bytes arrays not characters!) I can't read file based only on block size. Because it can be splitted wrong e.g. block size is 8 then
  //  position is 0
  //  Block content 49,13,10,50,13,10,51,13
  //  position is 8
  //  Block content 10,52,13,10,53,13,10,54
  //  Here you can see that 54(6) stay as last byte of block. 
  //  In case of number 10 is it possible that 49(1) will stay in one block but 48(0) in other
  //  it will lead to wrong result.
  public static class PFile
  {
    public const int N = 19452213;
    public static ushort nt = 10;
    public static long BlockSize = 10;
    private static long LastBlockSize = 0;
    public const string tf = @"D:\GoogleDrive\Study\notes\Programming\dotnet\examples\flow\test.txt";

    public static void Main(string[] args)
    {
      GenerateFile();
      // return;
      var s = new Stopwatch();
      s.Start();

      long l = 0;

      using (var f = new FileStream(tf, FileMode.Open, FileAccess.Read))
      {
        l = f.Length; // FIXME: I really have to get byte numbers before reading? Looks it's cost nothing.
      }

      Console.WriteLine($"Byte numbers {l}");

      var blockN = l / 8L;
      BlockSize = blockN / nt;
      LastBlockSize = 1;

      if (blockN % nt == 0) LastBlockSize = BlockSize;
      else nt += 2;

      Console.WriteLine($"Size of block {BlockSize}");
      Console.WriteLine($"Size of last block {LastBlockSize}");


      // if (BlockSize % 8 != 0 || LastBlockSize % 8 != 0) throw new InvalidOperationException("Size of block doesn't divide by 8!");


      var tsks = new Task<long>[nt];

      for (uint i = 0; i < tsks.Length; ++i)
      {
        var task_num = i;
        tsks[i] = Task.Run(() => GetBlockSum((long)task_num * BlockSize * 8));
      }

      Task.WaitAll(tsks.ToArray());
      Console.WriteLine($"par sum:  {tsks.Select(t => t.Result).Sum()}");
      Console.WriteLine($"Elapsed time for parallel reading is {s.Elapsed.TotalSeconds}");

      s.Restart();

      long sum = 0;
      using (var f = new FileStream(tf, FileMode.Open, FileAccess.Read))
      {
        byte[] b1 = new byte[f.Length];
        f.Read(b1, 0, b1.Length);
        for (int i = 0; i < f.Length; i += 8)
        {
          sum += BitConverter.ToInt64(b1, i);
        }
      }
      s.Stop();
      Console.WriteLine($"Sequenced sum is {sum}");
      Console.WriteLine($"Elapsed time for sequenced reading is {s.Elapsed.TotalSeconds}");
    }

    public static void GenerateFile()
    {
      using (var f = new FileStream(tf, FileMode.Create, FileAccess.Write))
      {
        for (long i = 1; i <= N; ++i)
        {
          f.Write(BitConverter.GetBytes(i));
        }
      }
    }

    public static long GetBlockSum(long position)
    {
      using (var f = new FileStream(tf, FileMode.Open, FileAccess.Read))
      {
        var bs = position > BlockSize * nt * 8 ? LastBlockSize : BlockSize;
        byte[] b1 = new byte[bs * 8L];
        f.Seek(position, SeekOrigin.Begin);
        f.Read(b1, 0, b1.Length);
        long sum = 0;

        for (int i = 0; i < b1.Length; i += 8)
        {
          long tmp = BitConverter.ToInt64(b1, i);
          // Console.WriteLine($"{Environment.NewLine} {Environment.NewLine} position is {position} {Environment.NewLine} Block content {string.Join(',', b1.Select(s => s.ToString()).ToArray())}. Value is {tmp}. BlockSize is {bs}");
          sum += tmp;
        }
        return sum;
      }
    }

  }

}
