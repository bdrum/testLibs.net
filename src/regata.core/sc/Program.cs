using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Regata.Core.GRPC.Xemo.Services;
using Regata.Core.GRPC.Xemo;
using Regata.Core;

namespace sc
{
    class Program
    {

        private static IReadOnlyDictionary<string, int> PairedXemoSN = new Dictionary<string, int>()
        {
            { "D1", 107374 },
            { "D2", 107375 },
            { "D3", 107376 },
            { "D4", 114005 },
        };
        // static async Task Main(string[] args)
        static void Main(string[] args)
        {
            Server.Run();
            // _ = Task.Run(() => Server.Run());
            // await Task.Delay(TimeSpan.FromSeconds(3));
            // await Task.WhenAll((new string[] { "D2", "D3", "D4" }).Select(async d => await Shell.ExecuteCommandAsync(@"D:\GoogleDrive\Job\flnp\dev\regata\Core\artifacts\Debug\XemoClient\XemoClient.exe", PairedXemoSN[d].ToString())));

            Console.WriteLine("Push any key to exit...");
            Console.ReadKey();
        }
    }
}
