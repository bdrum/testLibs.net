using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
namespace flow
{
    public class Garage : IEnumerable<Car>
    {
        private List<Car> cars = new List<Car>()
            {
            new Car(2, "merc"),
            new Car(3, "audi"),
            new Car(1, "bmw")
            };

        IEnumerator<Car> IEnumerable<Car>.GetEnumerator() => cars.GetEnumerator();

        // IEnumerator IEnumerable.GetEnumerator() => cars.GetEnumerator();
    }

    public class Car
    {
        public int Id;
        public string Mark;
        public Car(int id, string mark)
        {
            Id = id;
            Mark = mark;
        }
        public override string ToString()
        {
            return $"Id={Id}, Mark={Mark}";
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var w = new Stopwatch();
            w.Start();
            Console.WriteLine($"Run1 is starting:");
            Run1();
            w.Stop();
            Console.WriteLine($"Elapsed time: {w.Elapsed.TotalSeconds}");
            w.Restart();
            Console.WriteLine($"Run2 is starting:");
            Run2();
            Console.WriteLine($"Elapsed time: {w.Elapsed.TotalSeconds}");

            // var summary = BenchmarkRunner.Run<Sum>();

        }

    }
}

