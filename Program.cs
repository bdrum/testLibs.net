using System;
using System.Threading;
using System.Threading.Tasks;

namespace flow
{
    class Program
    {
        static async void Method1(CancellationToken ct)
        {
            try
            {
                Console.WriteLine("Parallel task is begining:");

                for (var i = 0; i < 10; ++i)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), ct);
                    Console.WriteLine($"{i}_task");
                }
            }
            catch (OperationCanceledException oce)
            {
                Console.WriteLine(oce.Message);
            }
        }

        static async Task Main(string[] args)
        {
            var canc = new CancellationTokenSource();
            Console.WriteLine("Let's start in parallel:");

            await Task.Run(() => Method1(canc.Token), canc.Token);


            for (var i = 0; i < 10; ++i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine($"{i}_main");
                if (i == 4)
                    canc.Cancel();
            }
            Console.ReadLine();
        }
    }
}
