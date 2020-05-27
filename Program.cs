using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace flow
{
    class Program
    {
        static void Main(string[] args)
        {

            var ancedent = new Task(() => 
            {
                Console.WriteLine("Here is ancedent task");
                throw new InvalidOperationException("Exception in ancedent task!");
            });

            ancedent.ContinueWith((ancedent) =>
            {
                if (ancedent.Exception != null)
                    foreach (var ie in ancedent.Exception.InnerExceptions)
                    {
                        if (ie is InvalidOperationException)
                            Console.WriteLine("Here is the continuation-handler for InvalidOperationException");
                    }
            }, TaskContinuationOptions.OnlyOnFaulted);

            ancedent.Start();

            Console.ReadLine();


        }

    }
}
