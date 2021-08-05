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

        static bool _completed = false;
        static async Task Main(string[] args)
        {
            Console.WriteLine("await MyAwaitable with _completed is true");
            _completed = true;
            Console.WriteLine(await new MyAwaitable());

            Console.WriteLine("await MyAwaitable with _completed is false");
            _completed = false;
            Console.WriteLine(await new MyAwaitable());
        }

        class MyAwaitable
        {
            public MyAwaiter GetAwaiter()
            {
                return new MyAwaiter();
            }
        }

        class MyAwaiter : INotifyCompletion
        {
            public void OnCompleted(Action cont)
            {
                Console.WriteLine("Called from OnCompleted()");
                cont();
            }

            public bool IsCompleted
            {
                get
                {
                    Console.WriteLine($"IsCompleted property has called. Value is {_completed}");
                    return _completed;
                }
            }

            public int GetResult()
            {
                Console.WriteLine("Result is 5. Called from GetResult()");
                return 5;
            }

        }
    }
}
