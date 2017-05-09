using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TwinkleStar.Common.XML
{
    public class XMLHelper
    {
        /// <summary>
        /// 创建一个不存在的XML文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="rootName">根节点名称</param>
        public static void CreateXML(string filePath, string rootName)
        {
            try
            {
                if (File.Exists(filePath))
                    throw new Exception("file already exist.");

                XDocument xDoc = new XDocument(new XElement(rootName));
                xDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载XML文件，不存在则创建
        /// </summary>
        /// <param name="filePath"></param>
        public static XDocument LoadOrCreateXML(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    XDocument xDoc = new XDocument(new XElement("Root"));
                    xDoc.Save(filePath);
                }

                return XDocument.Load(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建一个不存在的XML文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void CreateXML<TEntity>(string filePath, TEntity entity)
        {
            try
            {
                if (File.Exists(filePath))
                    throw new Exception("file already exist.");

                XDocument xDoc = new XDocument(new XElement(typeof(TEntity).Name));

                foreach (var member in typeof(TEntity).GetProperties()
                                                    .Where(i => i.PropertyType.IsValueType || i.PropertyType == typeof(String)))
                {
                    XElement xe = new XElement(member.Name, new XAttribute("value", member.GetValue(entity,null)));

                    xDoc.Root.Add(xe);
                }
                xDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
