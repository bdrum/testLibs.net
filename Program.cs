using System;
using System.Diagnostics;

namespace flow
{
    class Program 
    {
        static void Main(string[] args)
        {
            IShowDelegate conoleDelegate = new ConsoleHandler();
            IShowDelegate debugDelegate = new DebugHandler();

            var mo = new ModelOne();

            mo.showDelegate = conoleDelegate;
            mo.Message = "This is will be shown in console";

            mo.showDelegate = debugDelegate;
            mo.Message = "But this is will be shown id debug window.";

            mo.showDelegate = null;

            mo.ShowMeEvent += conoleDelegate.ShowMeTheMessage;
            mo.ShowMeEvent += debugDelegate.ShowMeTheMessage;

            mo.Message = "Messages in both sources via events!";

            Console.ReadLine();
        }
    }

    public class ModelOne : IShowDelegate
    {
        public IShowDelegate showDelegate;
        public event Action<string> ShowMeEvent;

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                showDelegate?.ShowMeTheMessage(value);
                ShowMeEvent?.Invoke(value);
                _message = value;

            }
        }

        void IShowDelegate.ShowMeTheMessage(string text) => showDelegate.ShowMeTheMessage(text);
    }

   public interface IShowDelegate
    {
        void ShowMeTheMessage(string text);
    }

    public class ConsoleHandler : IShowDelegate
    {
        void IShowDelegate.ShowMeTheMessage(string text) => Console.WriteLine(text);
    }

    public class DebugHandler : IShowDelegate
    {
        void IShowDelegate.ShowMeTheMessage(string text) => Debug.WriteLine(text);
    }
}
