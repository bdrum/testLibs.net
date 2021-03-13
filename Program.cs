using System;

namespace flow
{
  public class Program
  {
    public static void Main(string[] args)
    {
      B.Call();
      using (var a = new A())
      {
        B.Call();

      }
      B.Call();




    }

    public class A : IDisposable
    {
      public event Action AEvent;

      public void Dispose()
      {
      }

      public A()
      {
        B.BEvent += Call;
      }


      public void Call()
      {
        Console.WriteLine("Call from A");
      }
    }

    public static class B
    {
      public static void Call()
      {
        BEvent?.Invoke();
        Console.WriteLine("Call from B");
      }
      public static event Action BEvent;
    }


  }
}

