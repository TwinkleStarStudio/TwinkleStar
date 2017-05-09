using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common.Json;
using TwinkleStar.Data.LocalFileDB;

namespace TwinkleStar.Data.JsonRepository
{
    /// <summary>
    /// json仓储
    /// </summary>
    public class JsonRepository<TEntity> :
        FileDatabase<TEntity>, IJsonRepository<TEntity>
        where TEntity : JsonEntity,new()
    {
        string m_Directory = AppDomain.CurrentDomain.BaseDirectory;

        public new bool IsIndent
        {
            get
            {
                return base.IsIndent;
            }

            set
            {
                base.IsIndent = value;
            }
        }

        public JsonRepository(string directory)
            :base(directory)
        {
            FileExtension = @"db";
            this.m_Directory = directory;
        }

        public JsonRepository()
        {
            FileExtension = @"db";
            MyDirectory = m_Directory;
        }

        public int Delete(TEntity item)
        {
            Delete(item.RootID);

            return 1;
        }

        public TEntity Find(params object[] id)
        {
            return FindOneById(id[0].ToString());
        }

        public IQueryable<TEntity> GetModel()
        {
            return FindAll().AsQueryable();
        }

        public int Insert(TEntity item)
        {
            Save(item.RootID, item);

            return 1;
        }

        public int Update(TEntity item)
        {
            Save(item.RootID, item);

            return 1;
        }

        protected override string Serialize(object value, bool isIndent = false)
        {
            return JsonHelper.Serialize(value, isIndent);
        }

        protected override TEntity Deserialize(string data)
        {
            return JsonHelper.Deserialize<TEntity>(data);
        }
    }
}
