using System;
using System.Collections.Generic;
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
        static async Task Main(string[] args)
        {
            var ct1 = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
            SampleChanger[] s = null;
            try
            {
                s = new SampleChanger[] { new SampleChanger(107375), new SampleChanger(107374) };

                s[0].ErrorOccurred += (i, j) => { Console.WriteLine($"{i}, {j}"); };
                s[1].ErrorOccurred += (i, j) => { Console.WriteLine($"{i}, {j}"); };

                s[0].PositionReached += async (s) => await PositionReachedHandler(s);
                s[1].PositionReached += async (s) => await PositionReachedHandler(s);


                var tasks = new List<Task>();
                foreach (SampleChanger ss in s)
                {
                    SampleChanger x = ss;
                    tasks.Add(Task.Factory.StartNew((xx) => (xx as SampleChanger), x));
                }

                await Task.WhenAll(tasks);

                Console.WriteLine("We are home!");

                tasks.Clear();

                var ct2 = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;

                foreach (SampleChanger ss in s)
                {
                    SampleChanger x = ss;
                    tasks.Add(Task.Factory.StartNew((xx) => MoveToPosAsync(xx as SampleChanger), x));
                }

                await Task.WhenAll(tasks.ToArray());

                Console.WriteLine("We are at the position!");
                Console.ReadLine();


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

        private async static Task PositionReachedHandler(SampleChanger sc)
        {
            Console.WriteLine($"{sc.PairedDetector} has reached the position.Move next!");
            await MoveToPosAsync(sc);
        }

        private async static Task HomeAsync(SampleChanger sc)
        {
            using (var ct = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                await sc?.HomeAsync(ct.Token);
                Console.WriteLine($"{sc.PairedDetector} at home");
            }

        }

        private async static Task MoveToPosAsync(SampleChanger sc, int x = 30000)
        {
            if (sc.CurrentPosition.X == 30000)
                x = 60000;

            var ct = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            try
            {
                Console.WriteLine($"{sc.PairedDetector} is going to position");
                var p = new Position()
                {
                    X = x,
                    Y = 37300
                };
                await sc?.MoveToPositionAsync(p, moveAlongAxisFirst: Axes.X, ct.Token);
                Console.WriteLine($"{sc.PairedDetector} at position");

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch (TaskCanceledException)
            {
                await HomeAsync(sc);
            }
            finally
            {
                ct.Dispose();
            }
        }

        private static async Task XemoCycle(SampleChanger sc)
        {
            await HomeAsync(sc);
            await MoveToPosAsync(sc);

        }

        private static void Stop(SampleChanger sc)
        {
            sc?.Stop();
            Console.WriteLine($"{sc.PairedDetector} has stopped");
        }

    }
}