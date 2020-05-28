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
            Console.WriteLine("Here is ancedent task");
            var ancedent = Task.Run(() =>
            {
                Console.WriteLine("Here is ancedent task");
                throw new InvalidOperationException("Exception in ancedent task!");
            });


            try
            {
                ancedent.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    if (ex is InvalidOperationException)
                        Console.WriteLine(ex.Message);
                    return ex is InvalidOperationException;
                });
            }
        }

    }
}
