using System;
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
            SampleChanger s = null;
            try
            {
                s = new SampleChanger(114005);

                // s.PositionReached += () => { Console.WriteLine("Position reached!"); };

                var reg = new Regex(@"\d{1,2}");

                while (true)
                {
                    var l = Console.ReadLine();

                    if (l.Contains("put") && l.Contains("det"))
                    {
                        var cell = short.Parse(reg.Match(l).Value);
                        await s.TakeSampleFromTheCellAsync(cell);
                        await s.PutSampleAboveDetectorWithHeightAsync(Heights.h10);
                    }

                    if (l.Contains("put"))
                    {
                        var cell = short.Parse(reg.Match(l).Value);
                        await s.PutSampleToTheDiskAsync(cell);

                    }
                    else if (l.Contains("take"))
                    {
                        var cell = short.Parse(reg.Match(l).Value);
                        await s.TakeSampleFromTheCellAsync(cell);
                    }

                    else if (l.Contains("det"))
                    {
                        var h = Heights.h2p5;
                        if (l.Contains("5")) h = Heights.h5;
                        if (l.Contains("2.5")) h = Heights.h2p5;
                        if (l.Contains("20")) h = Heights.h20;
                        if (l.Contains("10")) h = Heights.h10;
                        await s.PutSampleAboveDetectorWithHeightAsync(h);
                    }

                    else if (l == "q")
                    {
                        break;
                    }
                    else if (l == "h")
                    {
                        await s.HomeAsync();
                        Console.WriteLine("I'm at home");
                    }
                    else if (l == "u")
                    {
                        s.MoveUp(5000);
                    }
                    else if (l == "d")
                    {
                        s.MoveDown(5000);
                    }
                    else if (l == "r")
                    {
                        s.MoveRight(5000);
                    }
                    else if (l == "l")
                    {
                        s.MoveLeft(5000);
                    }
                    else if (l == "cw")
                    {
                        s.MoveClockwise(5000);
                    }
                    else if (l == "ccw")
                    {
                        s.MoveСounterclockwise(5000);
                    }
                    else
                    {
                        s.Stop();
                    }
                }

                Console.WriteLine("Current position is");
                Console.WriteLine(s.CurrentPosition.ToString());

            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("The task was cancelled");
            }
            finally
            {
                s?.Stop();
                s?.Disconnect();
            }
        }
    }
}