using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreLibDotCore.ConfigHelper
{
    public class JsonConfig<T>: IBaseConfig<T>
    {
        private  object Obj = new object();
        public bool IsLog { get ; set ; }
        /// <summary>
        /// 生成配置文件
        /// </summary>
        /// <param name="path"></param>
        public async Task<bool> GenrateConfig(string path)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Type type = typeof(T);
                    var ob=  Activator.CreateInstance(type);
                    
                    var ss = JsonConvert.SerializeObject(ob);
                    File.WriteAllText(path, ss);
                    return true;
                }
                catch (Exception e)
                {
                    if (IsLog)
                    {
                        LogManager.AddLog(e);
                    }
                    return false;
                }
            });
        }

        public async Task<T> LoadConfig(string path)
        {
           return await Task.Run(() =>
           {
               try
               {
                   if (File.Exists(path))
                   {
                       string configStr;
                       lock (Obj)
                       {
                           configStr= File.ReadAllText(path);
                       }
                       var config = JsonConvert.DeserializeObject<T>(configStr);
                       return config;
                        
                   }

                   return default(T);
               }
               catch (Exception e)
               {
                   if (IsLog)
                   {
                       LogManager.AddLog(e);
                   }
                   return default(T);
               }
           });
        }

        public async Task SaveConfig(string path,T config)
        {
           await Task.Run(() =>
           {
               try
               {
                   var conJs = JsonConvert.SerializeObject(config);
                   lock (Obj)
                   {
                       File.WriteAllText(path, conJs);
                   }
               }
               catch (Exception e)
               {
                   if (IsLog)
                   {
                       LogManager.AddLog(e);
                   }
               }
           });
        }
    }
}
