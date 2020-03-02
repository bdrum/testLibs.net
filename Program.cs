using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace flow
{
    class Program
    {
        static void Method1()
        {
            for (var i = 0; i < 2; ++i)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Method1 is working...");
            }
        }

        static void Method2()
        {
            for (var i = 0; i < 4; ++i)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Method2 is working...");
            }
        }


        static void Method3()
        {
            Console.WriteLine("Start reading file async...");
            using (var file = new System.IO.StreamReader(@"D:\GoogleDrive\Study\notes\Programming\csharp\examples\flow\appsettings.json"))
                while (!file.EndOfStream)
                {
                    Console.WriteLine(file.ReadLine());
                    Thread.Sleep(1000);
                }
        }

        static void SlowMethod()
        {
            Method1();
            Method2();
            Method3();

            Console.WriteLine($"Slow method completed");

        }

        static void Main(string[] args)
        {
            var st = Stopwatch.StartNew();
            Console.WriteLine($"Start Main sync.:");
            SlowMethod();

            for (var i = 0; i < 5; ++i)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"CallMethod is working...");
            }

            st.Stop();
            Console.WriteLine($"Main method has finished working");
            Console.WriteLine($"Elapsed time {st.ElapsedMilliseconds / 1000} sec");
        }
    }
}
