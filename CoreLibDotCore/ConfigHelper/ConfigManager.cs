using System.Threading.Tasks;

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
        /// <param name="configType">json 或Xml</param>
        /// <param name="isLog">发生错误时，是否打印日志，开启可能会降低一些性能</param>
        public static void  Init(string path,ConfigType configType,bool isLog)
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

            ConfigGenr.IsLog = isLog;
        }

        /// <summary>
        /// 加载配置文件，并对实体对象赋值
        /// </summary>
        /// <returns></returns>
        public static async Task<T> LoadAsync()
        {
            var config= await ConfigGenr.LoadConfig(SavePath);
            Config = config;
            return config;
        }

        /// <summary>
        /// 不带参保存配置文件默认，使用实体值
        /// </summary>
        public static async Task Save()
        {
             await ConfigGenr.SaveConfig(SavePath,Config);
        }


        /// <summary>
        /// 生成配置文件,在已经有配置的实体对象时才能生成
        /// </summary>
        public static async Task<bool> GenraConfig()
        {
              return await ConfigGenr.GenrateConfig(SavePath);
        }

      }


 }

