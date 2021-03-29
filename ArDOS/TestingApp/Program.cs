using System;
using ArdosModel;

namespace TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BashAction("echo 'hello world!'", new string[] { "param1", "param2" });
            Console.WriteLine(ba.ActionType);
        }
    }
}
