using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using Regata.Core.Hardware;
using Regata.Core.DataBase.Models;

namespace WorkFlowCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            var ct = new CancellationTokenSource();
            Task.Run(async () => await Run(ct.Token));
            while (true)
            {
                var l = Console.ReadLine();
                if (l.Contains("q"))
                {
                    ct.Cancel();
                    break;
                }
            }
        }

        static async Task Run(CancellationToken ct)
        {

            SampleChanger[] s = null;
            try
            {
                s = new SampleChanger[] { new SampleChanger(107374), new SampleChanger(107375) };

                await Task.WhenAll(s.Select(async ss => await HomeAsync(ss, ct)));
                Console.WriteLine("We are home!");
                await Task.WhenAll(s.Select(async ss => await MoveToPos(ss, ct)));
                Console.WriteLine("We are at the position!");


            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("The task was cancelled");
                await Task.WhenAll(s.Select(ss => Task.Run(() => Stop(ss))));

            }
            finally
            {
                foreach (var sc in s)
                {
                    sc.Stop();
                    sc.Disconnect();
                }
            }
        }

        private static async Task HomeAsync(SampleChanger sc, CancellationToken ct)
        {
            await sc?.HomeAsync(ct);
            Console.WriteLine($"{sc.PairedDetector} at home");

        }

        private static async Task MoveToPos(SampleChanger sc, CancellationToken ct)
        {
            var p = new Position()
            {
                X = 50000,
                Y = 37300
            };
            await sc?.MoveToPositionAsync(p, moveAlongAxisFirst: Axes.X, ct);
            Console.WriteLine($"{sc.PairedDetector} at position");

        }

        private static void Stop(SampleChanger sc)
        {
            sc?.Stop();
            Console.WriteLine($"{sc.PairedDetector} has stopped");
        }

    }
}