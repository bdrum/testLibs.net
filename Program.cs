using System;
using System.Threading.Tasks;
using System.Threading;

namespace flow
{
    class Program
    {
        static async Task DoSomeThingAsync()
        {
            Console.WriteLine("DoSomeThingAsync:This was call synchroniously");

            //await Task.Delay(2000).ConfigureAwait(false);
            await Task.Delay(5000);

            Console.WriteLine("DoSomeThingAsyncThis was call after async pause");
        }

        static void DoSomeThing(string a)
        {
            Console.WriteLine(a);
            System.Threading.Thread.Sleep(5000);
        }

        static void Main(string[] args)
        {
            CallMe();

            Console.WriteLine("This called after 'CallMe'");
            Console.ReadLine();
        }
        static async Task CallMe()
        {
            Console.WriteLine("CallMe:Start app:");
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var timeout = Task.Delay(Timeout.InfiniteTimeSpan, cts.Token);

            var task = new Task<Task>(DoSomeThingAsync, cts.Token);
            var task1 = new Task(() => DoSomeThing("this is arg"), cts.Token);
            var t = await Task.WhenAny(timeout, task);

            if (t == timeout)
                Console.WriteLine("TimeOut!");

            //await DoSomeThingAsync();

            Console.WriteLine("End app");

        }
    }
}
