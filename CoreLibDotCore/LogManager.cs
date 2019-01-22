using System;
using System.Globalization;
using System.IO;

namespace CoreLibDotCore
{
    public class LogManager
    {
        private static readonly string StorePath = $"{AppDomain.CurrentDomain.BaseDirectory}" + "log";
        private static readonly object Obj = new object();
        public static void AddLog(string str)
        {
            CheckPath(StorePath);
            string error = DateTime.Now.ToString(CultureInfo.InstalledUICulture) + " | " + "日志信息:  " + str + "\r\n";
            string dateTime = DateTime.Now.ToShortDateString();
            dateTime = dateTime.Replace("/", "");
            try
            {

                lock (Obj)
                {
                    using (FileStream fs = new FileStream(StorePath + $"/{dateTime}.txt", FileMode.Append))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(error);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

        public static void AddLog(Exception e)
        {
            CheckPath(StorePath);
            string error = DateTime.Now.ToString(CultureInfo.InstalledUICulture) + " | " + "错误信息：" + e.Message + "\r\n导致错误的对象名称:" +
                           e.Source + "\r\n引发异常的方法:" +
                           e.TargetSite + "\r\n帮助链接:" +
                           e.HelpLink + "\r\n调用堆:" +
                           e.StackTrace + "\r\n";
            string dateTime = DateTime.Now.ToShortDateString();
            dateTime = dateTime.Replace("/", "");
            try
            {
                lock (Obj)
                {
                    using (FileStream fs = new FileStream(StorePath + $"/{dateTime}.txt", FileMode.Append))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(error);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }


        }

        public static void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


        }
    }
}
