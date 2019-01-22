using System;
using System.Threading;
using CoreLibDotCore.ActionHelper;

namespace DotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Action hello = new Action((() =>
            {
              Thread.Sleep(10000);
            }));
            MethodHelper.AddAction("Add", hello);
            MethodHelper.InvokeAction("Add");
            Console.Read();
        }
    }
}
