using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace flow
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new A("123A");
            a.Show();
            var b = new B("123B");
            b.Show();
            Console.WriteLine(b.i);
            Console.WriteLine(a.i);
        }

        public struct A
        {
            public int i = 300;

            public A(string a) => Console.WriteLine(a);
            public void Show() => Console.WriteLine("Show A");
        }

        public class B
        {
            public int i = 30;
            public B(string a) => Console.WriteLine(a);
            public void Show() => Console.WriteLine("Show B");
        }
    }
}
