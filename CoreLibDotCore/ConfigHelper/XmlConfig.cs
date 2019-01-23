using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreLibDotCore.ConfigHelper
{
    public class XmlConfig<T> : IBaseConfig<T>
    {
        private T _config;
        private string _savePath;
        private object Obj = new object();
        public async Task<bool> GenrateConfig(string path)
        {
            return await Task.Run((() =>
            {
                try
                {
                    Type type = typeof(T);
                    
                    XElement xElement= CreateXelement(type);
                    XDocument document=new XDocument(new XComment("配置文件"),
                    xElement);
                    document.Save(path);
                    return true;

                }
                catch (Exception e)
                {
                       LogManager.AddLog(e);
                       return false;
                }

            }));
        }

        public async Task<T> LoadConfig(string path)
        {
          return await  Task.Run((() =>
            {
                try
                {
                    if (File.Exists(path))
                    {
                        XDocument docu;
                        lock (Obj)
                        {
                            docu = XDocument.Load(path);
                        }

                        XElement root = docu.Root;
                        var entity = CreateEntity(root, typeof(T));
                        return (T) entity;


                    }
                    return default(T);
                }
                catch (Exception e)
                {
                    LogManager.AddLog(e);
                    return default(T);
                }
            }));
        }

        public async Task SaveConfig(string path, T config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建节点元素
        /// </summary>
        public XElement CreateXelement(Type type)
        {
            var className = type.Name;
            var propers = type.GetProperties();
            XElement[] xElements=new XElement[propers.Length];
         
            for (int i = 0; i < propers.Length; i++)
            {
                XElement childElement=null;
                if (!propers[i].PropertyType.IsPrimitive && propers[i].PropertyType != typeof(string)&&!propers[i].PropertyType.IsValueType)  //不是是基本类型且不是string
                {
                  childElement=CreateXelement(propers[i].PropertyType);
                }
                else
                {
                    childElement = new XElement(propers[i].Name, "");
                }
                xElements[i] = childElement;
            }
            XElement xElement = new XElement($"{className}",xElements);
            return xElement;


        }


        //应该通过发现Xelement来给泛型类型赋值
        public object CreateEntity(XElement element,Type type)
        {
            var entity = Activator.CreateInstance(type);
            var type1 = entity.GetType();
            var elemens= element.Elements();
            foreach (var xElement in elemens)
            {
                if (xElement.HasElements)
                {
                   var childClass= type1.GetProperty(xElement.Name.ToString()); 
                   CreateEntity(xElement,childClass.PropertyType);
                }
                else
                {
                    type1.GetProperty(xElement.Name.ToString())?.SetValue(entity, xElement.Value,null);
                }
            }

            return entity;
        }
    }
}
