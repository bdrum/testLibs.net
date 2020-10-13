using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace flow
{
    class Program
    {
        public static int BlockSize = 0;
        public static int ListProp { get; set; }

        public static async Task Main(string[] args)
        {

            var l = new List<int>() { 1, 2, 3 };


            for (var i = 0; i < 10; ++i)
            {
                Inc(ref ListProp);
                Inc(ref l[i]);
                Console.WriteLine(i);
            }

            return;



            var s = new Stopwatch();
            s.Start();

            long l = 0;

            string pth = @"D:\GoogleDrive\Study\notes\Programming\dotnet\examples\flow\test.txt";

            using (var f = new FileStream(pth, FileMode.Open, FileAccess.Read))
            {
                l = f.Length;
            }
            Console.WriteLine(l);
            Console.WriteLine("answer: 5000000500000");

            BlockSize = 10000;
            var tsks = new Task<long>[(int)(l / BlockSize)];

            for (long i = 0; i < tsks.Length; ++i)
            {
                tsks[i] = Task.Run(() => GetBlockSum(i));
            }

            Task.WaitAll(tsks.ToArray());
            Console.WriteLine($"par sum:  {tsks.Select(t => t.Result).Sum()}");
            Console.WriteLine(s.ElapsedMilliseconds);

            s.Restart();

            long sum = 0;

            for (long i = BlockSize; i < l; i += BlockSize)
            {
                sum += GetBlockSum(i);
            }
            Console.WriteLine($"seq sum:  {sum}");
            Console.WriteLine(s.ElapsedMilliseconds);

            s.Restart();

            var arrs = File.ReadAllLines(pth);
            long ssm = arrs.AsParallel().Where(s => long.TryParse(s, out _)).Select(i => long.Parse(i)).Sum();
            Console.WriteLine($"readlines sum:  {sum}");


            Console.WriteLine(s.ElapsedMilliseconds);


        }

        public static void Inc(ref int i)
        {
            i++;
        }

        public static void GenerateFile()
        {
            var pth = @"D:\GoogleDrive\Study\notes\Programming\dotnet\examples\flow\test.txt";
            File.WriteAllLines(pth, Enumerable.Range(1, 10000000).ToList().ConvertAll(i => i.ToString()));
        }

        public static long GetBlockSum(long position)
        {
            using (var f = new FileStream(@"D:\GoogleDrive\Study\notes\Programming\dotnet\examples\flow\test.txt", FileMode.Open, FileAccess.Read))
            {
                byte[] b1 = new byte[BlockSize];
                f.Seek(position, SeekOrigin.Begin);
                f.Read(b1, 0, b1.Length);
                return Encoding.Default.GetString(b1).Split("\n").Where(s => long.TryParse(s, out _)).Select(i => long.Parse(i)).Sum();   
            }
        }
    }
}
