using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using TwinkleStar.Common.XML;

namespace TwinkleStar.Data.XMLRepository
{
    /// <summary>
    /// XML文件数据仓储
    /// XML结构为Attribute eg. <User id="1" name="zzl" />
    /// </summary>
    public class XML2Repository<TEntity> :
        IXMLRepository<TEntity>
        where TEntity : XMLEntity, new()
    {
        XDocument m_doc;
        string m_filePath;
        static object lockObj = new object();

        public XML2Repository(string filePath)
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
                           where db.Attribute("RootID").Value == item.RootID
                           select db).Single();

            xe.Remove();
            lock (lockObj)
            {
                this.m_doc.Save(this.m_filePath);
            }

            return 1;
        }

        public TEntity Find(params object[] id)
        {//todo
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
                        member.SetValue(entity, Convert.ChangeType(item.Attribute(member.Name).Value, member.PropertyType), null);
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
                db.Add(new XAttribute(member.Name, member.GetValue(item,null) ?? string.Empty));
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
                           where db.Attribute("RootID").Value == item.RootID
                           select db).Single();

            try
            {
                foreach (var member in item.GetType()
                                           .GetProperties()
                                           .Where(i => i.PropertyType.IsValueType
                                               || i.PropertyType == typeof(String)))
                {
                    xe.SetAttributeValue(member.Name, member.GetValue(item, null) ?? string.Empty);
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
