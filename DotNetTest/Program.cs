using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using CoreLibDotCore.ActionHelper;
using CoreLibDotCore.ConfigHelper;
using Newtonsoft.Json;

namespace DotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var type = typeof(MyClass);
            //var cons = type.GetConstructors();
            //Stopwatch wa=new Stopwatch();
            //wa.Start();
            //for (int i = 0; i < 1000000; i++)
            //{
            //    Activator.CreateInstance(type);
            //}
            //Console.WriteLine(wa.ElapsedMilliseconds);
            //for (int i = 0; i < 100000; i++)
            //{
            //       type.Assembly.CreateInstance(type.FullName);
            //}
            //Console.WriteLine(wa.ElapsedMilliseconds);
            //for (int i = 0; i < 100000; i++)
            //{
            //    cons[1].Invoke(new object[] { });
            //}
            //Console.WriteLine(wa.ElapsedMilliseconds);
            //Console.Read();
            //Console.WriteLine("Hello World!");


            //
       
            ConfigManager<MyClass>.Init(AppDomain.CurrentDomain.BaseDirectory+"1.xml",ConfigType.Xml);
            var jie=ConfigManager<MyClass>.LoadAsync().Result;
          //  ConfigManager<MyClass>.GenraConfig().Wait();
            // MyClass rre= ConfigManager<MyClass>.LoadAsync().Result;
            ConfigManager<MyClass>.Save().Wait();
           

           
            JsonConfig<MyClass> mConfig=new JsonConfig<MyClass>();
            Action hello = new Action((() =>
            {
              Thread.Sleep(10000);
            }));
            MethodHelper.AddAction("Add", hello);
            MethodHelper.InvokeAction("Add");
            Console.Read();
        }
    }

    /// <summary>
    /// 你好，我的兄弟
    /// </summary>
    class MyClass
    {
        
        public  int Age { get; set; }
        public  string Name { get; set; }
        public Child Hello { get; set; }

        public MyClass(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 你的
        /// </summary>
        public MyClass()
        {

        }
    }

    class Child
    {
        public string Name { get; set; }
        public Color HeaderColor { get; set; }
    }
}
