using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace flow
{
    class CustomData
    {
        public long CreationTime;
        public int Name;
        public int ThreadNum;
    }

    public class Program
    {
        static async Task Task1()
        {
            Console.WriteLine("This is the first task");
            await Task.Delay(TimeSpan.FromSeconds(6));
            Console.WriteLine("First task has done");
        }

        static async Task Task2()
        {
            Console.WriteLine("This is the second task");
            await Task.Delay(TimeSpan.FromSeconds(2));
            Console.WriteLine("Second task has done");
        }

        static async Task Task3()
        {
            Console.WriteLine("This is the third task");
            await Task.Delay(TimeSpan.FromSeconds(4));
            Console.WriteLine("Third task has done");
        }

        static void Main(string[] args)
        {
            MainMain();
            Console.ReadLine();
        }

        static async void MainMain()
        {
            var t1 = Task1();
            var t2 = Task2();
            var t3 = Task3();

            var tasks = new List<Task> { t1, t2, t3 };

            while (tasks.Any())
            {
                var t = await Task.WhenAny(tasks);

                if (t == t1)
                    Console.WriteLine($"Now I can run code specially for the first task");
                if (t == t2)
                    Console.WriteLine($"Now I can run code specially for the second task");
                if (t == t3)
                    Console.WriteLine($"Now I can run code specially for the third task");

                tasks.Remove(t);
            }

            Console.WriteLine("This is last main output");
        }
    }
}
