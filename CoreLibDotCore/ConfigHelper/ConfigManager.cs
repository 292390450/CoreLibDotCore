using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreLibDotCore.ConfigHelper
{
    public class ConfigManager<T>
    {
        public static T Config;
        public static IBaseConfig<T> ConfigGenr;
        public static  ConfigType ConfigType;
        public static string SavePath;
        /// <summary>
        /// 初始化配置文件管理类
        /// </summary>
        /// <param name="path">路径应为全路径，包含文件名</param>
        /// <param name="configType"></param>
        public static void  Init(string path,ConfigType configType)
        {
            SavePath = path;
            ConfigType = configType;
            switch (configType)
            {
                case ConfigType.Json:
                    ConfigGenr = new JsonConfig<T>();
                    break;
                case ConfigType.Xml:
                    ConfigGenr=new XmlConfig<T>();
                    break;
            }
        }

        public static async Task<T> LoadAsync()
        {
            return await ConfigGenr.LoadConfig(SavePath);
        }

        /// <summary>
        /// 不带参保存配置文件默认，使用
        /// </summary>
        public static async Task Save()
        {
             await ConfigGenr.SaveConfig(SavePath,Config);
        }


        /// <summary>
        /// 生成配置文件
        /// </summary>
        public static async Task<bool> GenraConfig()
        {
              return await ConfigGenr.GenrateConfig(SavePath);
        }

      }


 }

