using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CoreLibFrame4
{
    public class HttpHelper
    {
        public static  T Get<T>(string uri, string baseUri = "")
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(baseUri + uri);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = 60000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                string strResult =  streamReader.ReadToEnd();
                streamReader.Close();
                streamReceive.Close();
                request.Abort();
                response.Close();
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(strResult, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(strResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                LogManager.AddLog(e);
                return default(T);
            }
        }
        public static T Post<T>(string uri, string para)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 60000;

                request.UserAgent = "DefaultUserAgent";
                string paras = para;
                if (!string.IsNullOrEmpty(paras))
                {
                    byte[] data = Encoding.Default.GetBytes(paras);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream streamReceive = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                    string res = streamReader.ReadToEnd();
                    streamReader.Close();
                    streamReceive.Close();
                    response.Close();
                    request.Abort();
                    if (typeof(T) == typeof(string))
                    {
                        return (T)Convert.ChangeType(res, typeof(T));
                    }
                    return JsonConvert.DeserializeObject<T>(res);
                }
                request.Abort();
                return default(T);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                 LogManager.AddLog(e);
                return default(T);
            }
        }
    }
}
