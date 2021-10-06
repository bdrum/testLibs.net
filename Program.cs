using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace flow
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("This is the initial point");


            var t = new Program();
            // await t.Meth();

            // var t1 = new Task(async () => await t.Meth());
            // var t2 = new Task(async () => await t.Meth());
            // var t3 = new Task(async () => await t.Meth());

            // t1.Start();
            // t2.Start();
            // t3.Start();
            await Task.WhenAll(t.Meth(), t.Meth(), t.Meth());

            Console.WriteLine("This is the end");
            Console.ReadLine();

        }

        private async Task RunAllMeth()
        {

        }
        private async Task Meth()
        {

            for (var i = 0; i < 10; ++i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine(i);
            }

        }




    }
}