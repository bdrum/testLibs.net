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
            // Run(107374), Run(107375), Run(107376), Run(114005)   
            await Task.WhenAll(Run(114005));
            Console.WriteLine("Press enter for exit...");
            Console.ReadLine();
        }
        static async Task Run(int id)
        {

            var s1 = new SampleChanger(id);
            try
            {

                s1.ErrorOccurred += (i, j) => { Console.WriteLine($"{i}, {j}"); };

                s1.PositionReached += async (s11) => await PositionReachedHandler(s11);

                await XemoCycle(s1);

                await Task.Delay(TimeSpan.FromMinutes(1));

            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("The task was cancelled");
                s1.Stop();
            }
            finally
            {
                s1.Stop();
                s1.Disconnect();
            }
        }

        private static async Task PositionReachedHandler(SampleChanger sc)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine($"{sc.PairedDetector} has reached the position.Move next!");
            await MoveToPosAsync(sc);
        }

        private static async Task HomeAsync(SampleChanger sc)
        {
            try
            {
                //SampleChanger.ComSelect(sc.ComPort);

                Console.WriteLine($"Xemo {sc.PairedDetector} is going to home");
                using (var ct = new CancellationTokenSource(TimeSpan.FromSeconds(45)))
                {
                    await sc?.HomeAsync(ct.Token);
                    Console.WriteLine($"{sc.PairedDetector} at home");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Home timeout exceeds");
            }
        }

        private static async Task MoveToPosAsync(SampleChanger sc, int x = 30000)
        {
            //SampleChanger.ComSelect(sc.ComPort);

            if (sc.CurrentPosition.X == 30000)
                x = 60000;
            try
            {
                using (var ct = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
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
            }
            catch (TaskCanceledException)
            {
                // await XemoCycle(sc);
            }
        }

        private static async Task XemoCycle(SampleChanger sc)
        {
            //SampleChanger.ComSelect(sc.ComPort);

            // sc.PositionReached += async (s) => await PositionReachedHandler(sc);
            await HomeAsync(sc);
            await MoveToPosAsync(sc);
        }
    }
}