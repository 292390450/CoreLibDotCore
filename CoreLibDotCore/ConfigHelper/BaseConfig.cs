using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibDotCore.ConfigHelper
{
    public interface IBaseConfig<T>
    {
          Task<bool> GenrateConfig(string path);
          Task<T> LoadConfig(string path);
          Task SaveConfig(string path, T config);
    }
}
