using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Common.ZTest
{
    public class AsyncTest : ITest
    {
        public async void Test()
        {
            var normal = new Normal();
            var result = await normal.DoSomethingAsync("Damn it");
            Console.WriteLine(result);
            //Asynchronous Pattern: BeginXXX and EndXXX (delegate default support it : Invoke)
            //Event-Asynchronous-Pattern Backgroundworker, XXXRunAsync
            //Task-Asynchronous-Pattern async await
        }
    }

    public class Normal
    {
        public string DoSomething(string input)
        {
            Thread.Sleep(5000);
            return input.ToUpper();
        }
    }

    public static class NormalExtension
    {
        private static Func<string, string> _doSomething;

        public static Task<string> DoSomethingAsync(this Normal normal, string input)
        {
            _doSomething = normal.DoSomething;
            return Task<string>.Factory.FromAsync(
                (s, callback, state) => _doSomething.BeginInvoke(s, callback, state),
                ar => _doSomething.EndInvoke(ar),
                input,
                null);
        }
    }
}
