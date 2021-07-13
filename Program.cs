using System;
using System.Collections.Generic;
namespace flow
{
    public class Program
    {

        static IEnumerator<int> func()
        {
            for (int i = 0; i < 10; ++i)
                yield return i;
        }


        public static void Main(string[] args)
        {

            Console.WriteLine(func().Current);

        }


    } // public class Program
}   // namespace flow

