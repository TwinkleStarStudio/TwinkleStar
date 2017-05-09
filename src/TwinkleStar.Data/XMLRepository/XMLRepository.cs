using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using TwinkleStar.Common.XML;

namespace TwinkleStar.Data.XMLRepository
{
    /// <summary>
    /// XML文件数据仓储
    /// XML结构为XElement eg. 
    /// <User>
    /// <id>1</id>
    /// <name>zzl</name>
    ///</User>
    ///</summary>
    public class XMLRepository<TEntity> :
        IXMLRepository<TEntity>
        where TEntity : XMLEntity, new()
    {
        XDocument m_doc;
        string m_filePath;
        static object lockObj = new object();

        /// <summary>
        /// 文件路径
        /// </summary>
        /// <param name="filePath"></param>
        public XMLRepository(string filePath)
        {
            this.m_filePath = filePath;

            this.m_doc = XMLHelper.LoadOrCreateXML(filePath);
        }

        public int Delete(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("The database entity can not be null.");
            }

            XElement xe = (from db in this.m_doc.Root.Elements(typeof(TEntity).Name)
                           where db.Element("RootID").Attribute("value").Value == item.RootID
                           select db).Single();

            xe.Remove();
            lock (lockObj)
            {
                this.m_doc.Save(this.m_filePath);
            }

            return 1;
        }

        public TEntity Find(params object[] id)
        {
            return GetModel().FirstOrDefault(i => i.RootID == Convert.ToString(id[0]));
        }

        public IQueryable<TEntity> GetModel()
        {
            IEnumerable<XElement> list = this.m_doc.Root.Elements(typeof(TEntity).Name);
            IList<TEntity> returnList = new List<TEntity>();

            foreach (var item in list)
            {
                TEntity entity = new TEntity();
                foreach (var member in entity.GetType()
                                             .GetProperties()
                                             .Where(i => i.PropertyType.IsValueType || i.PropertyType == typeof(String)))
                {
                    if (item.Attribute(member.Name) != null)
                    {
                        member.SetValue(entity, Convert.ChangeType(item.Element(member.Name).Attribute("value").Value, member.PropertyType), null);
                    }
                }
                returnList.Add(entity);
            }

            return returnList.AsQueryable();
        }

        public int Insert(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("The database entity can not be null.");
            }

            XElement db = new XElement(typeof(TEntity).Name);
            //反射获取属性
            foreach (var member in item.GetType()
                                       .GetProperties()
                                       .Where(i => i.PropertyType.IsValueType || i.PropertyType == typeof(String)))
            {
                db.Add(new XElement(member.Name, new XAttribute("value", member.GetValue(item, null))));
            }

            this.m_doc.Root.Add(db);
            lock (lockObj)
            {
                this.m_doc.Save(this.m_filePath);
            }

            return 1;
        }

        public int Update(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("The database entity can not be null.");
            }

            XElement xe = (from db in this.m_doc.Root.Elements(typeof(TEntity).Name)
                           where db.Element("RootID").Attribute("value").Value == item.RootID
                           select db).Single();

            try
            {
                foreach (var member in item.GetType()
                                           .GetProperties()
                                           .Where(i => i.PropertyType.IsValueType
                                               || i.PropertyType == typeof(String)))
                {
                    xe.Add(new XElement(member.Name, new XAttribute("value", member.GetValue(item, null))));
                }
                lock (lockObj)
                {
                    this.m_doc.Save(this.m_filePath);
                }
            }
            catch
            {
                throw;
            }

            return 1;
        }
    }
}
