using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
                    XElement xElement= CreateXelement(type,type.Name);
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


        /// <summary>
        /// 使用创建一个xml文档保存，还是加载本地文档进行修改？ 先使用直接创建一个覆盖保存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task SaveConfig(string path, T config)
        {
            var type = typeof(T);
            var xml= CreateXelement(type, type.Name);


        }

        /// <summary>
        /// 创建节点元素
        /// </summary>
        public XElement CreateXelement(Type type,string name)
        {
            
            var propers = type.GetProperties();
            XElement[] xElements=new XElement[propers.Length];
         
            for (int i = 0; i < propers.Length; i++)
            {
                XElement childElement=null;
                if (!propers[i].PropertyType.IsPrimitive && propers[i].PropertyType != typeof(string)&&!propers[i].PropertyType.IsValueType)  //不是是基本类型且不是string
                {
                  childElement=CreateXelement(propers[i].PropertyType,propers[i].Name);
                }
                else
                {
                    childElement = new XElement(propers[i].Name, "");
                }
                xElements[i] = childElement;
            }
            XElement xElement = new XElement($"{name}",xElements);
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
                PropertyInfo propertyInfo=null;
                try
                {
                    propertyInfo = type1.GetProperty(xElement.Name.ToString());
                }
                catch (Exception e)
                {
                    LogManager.AddLog(e);
                }

                if (xElement.HasElements&&propertyInfo!=null)
                {
                   var child= CreateEntity(xElement, propertyInfo.PropertyType);
                   propertyInfo.SetValue(entity,child,null);
                   
                }
                else
                {
                    if (!string.IsNullOrEmpty(xElement.Value)&& propertyInfo!= null)   //没有值，不必要转换
                    {
                        try
                        {
                            object v = Convert.ChangeType(xElement.Value, propertyInfo.PropertyType);
                            propertyInfo?.SetValue(entity, v, null);
                        }
                        catch (Exception e)
                        {
                            LogManager.AddLog(e);
                        }
                    }
                }
            }

            return entity;
        }
    }
}
