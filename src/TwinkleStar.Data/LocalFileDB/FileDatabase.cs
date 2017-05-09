using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TwinkleStar.Common;

namespace TwinkleStar.Data.LocalFileDB
{
    /// <summary>
    /// 文件数据库，这是一个抽象类。(之前做的拿来用的，也可以应用于xml,但已有linq to xml，所以这边暂时仅用于json)
    /// </summary>
    public abstract class FileDatabase<TEntity>
    {
        #region Fields

        /// <summary>
        /// 文件数据库操作锁
        /// </summary>
        protected static readonly object operationLock = new object();
        private static HashSet<char> invalidFileNameChars;

        static FileDatabase()
        {
            invalidFileNameChars = new HashSet<char>() { '\0', ' ', '.', '$', '/', '\\' };
            foreach (var c in Path.GetInvalidPathChars()) { invalidFileNameChars.Add(c); }
            foreach (var c in Path.GetInvalidFileNameChars()) { invalidFileNameChars.Add(c); }
        }

        /// <summary>
        /// 文件数据库
        /// </summary>
        /// <param name="directory">数据库文件所在目录</param>
        protected FileDatabase(string directory)
        {
            MyDirectory = directory;
        }

        public FileDatabase()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 数据库文件所在目录
        /// </summary>
        public virtual string MyDirectory { get; protected set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public virtual string FileExtension { get; set; }

        public virtual bool IsIndent { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <param name="id">文档ID</param>
        /// <param name="document">文档对象</param>
        /// <returns>文档ID</returns>
        public virtual string Save(string id, TEntity document)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            if (document == null)
                throw new ArgumentNullException("document");

            Delete(id);

            try
            {
                string fileName = GenerateFileFullPath(id);
                string output = Serialize(document, IsIndent);

                lock (operationLock)
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(fileName);
                    System.IO.Directory.CreateDirectory(info.Directory.FullName);
                    System.IO.File.WriteAllText(fileName, output);
                }
            }
            catch (Exception ex)
            {
                throw new FileDatabaseException(
                  string.Format(CultureInfo.InvariantCulture,
                  "Save document failed with id [{0}].", id), ex);
            }

            return id;
        }

        /// <summary>
        /// 根据文档ID查找文档
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <param name="id">文档ID</param>
        /// <returns>文档对象</returns>
        public virtual TEntity FindOneById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            try
            {
                string fileName = GenerateFileFullPath(id);
                if (File.Exists(fileName))
                {
                    string fileData = File.ReadAllText(fileName);
                    return Deserialize(fileData);
                }

                return default(TEntity);
            }
            catch (Exception ex)
            {
                throw new FileDatabaseException(
                  string.Format(CultureInfo.InvariantCulture,
                  "Find document by id [{0}] failed.", id), ex);
            }
        }

        /// <summary>
        /// 查找指定类型的所有文档
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <returns>文档对象序列</returns>
        public virtual IEnumerable<TEntity> FindAll()
        {
            try
            {
                List<TEntity> list = new List<TEntity>();

                if (Directory.Exists(GenerateFilePath()))
                {
                    string[] files = System.IO.Directory.GetFiles(
                      GenerateFilePath(),
                      "*." + FileExtension,
                      SearchOption.TopDirectoryOnly);

                    foreach (string fileName in files)
                    {
                        string fileData = File.ReadAllText(fileName);
                        TEntity document = Deserialize(fileData);
                        if (document != null)
                        {
                            list.Add(document);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new FileDatabaseException(
                  "Find all documents failed.", ex);
            }
        }

        /// <summary>
        /// 根据指定文档ID删除文档
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <param name="id">文档ID</param>
        public virtual void Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            try
            {
                string fileName = GenerateFileFullPath(id);
                if (File.Exists(fileName))
                {
                    lock (operationLock)
                    {
                        File.Delete(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileDatabaseException(
                  string.Format(CultureInfo.InvariantCulture,
                  "Delete document by id [{0}] failed.", id), ex);
            }
        }

        /// <summary>
        /// 删除所有指定类型的文档
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        public virtual void DeleteAll()
        {
            try
            {
                if (Directory.Exists(GenerateFilePath()))
                {
                    string[] files = System.IO.Directory.GetFiles(
                      GenerateFilePath(), "*." + FileExtension,
                      SearchOption.TopDirectoryOnly);

                    foreach (string fileName in files)
                    {
                        lock (operationLock)
                        {
                            File.Delete(fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileDatabaseException(
                  "Delete all documents failed.", ex);
            }
        }

        /// <summary>
        /// 获取指定类型文档的数量
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <returns>文档的数量</returns>
        public virtual int Count()
        {
            try
            {
                if (Directory.Exists(GenerateFilePath()))
                {
                    string[] files = System.IO.Directory.GetFiles(
                      GenerateFilePath(),
                      "*." + FileExtension, SearchOption.TopDirectoryOnly);
                    if (files != null)
                    {
                        return files.Length;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new FileDatabaseException(
                  "Count all documents failed.", ex);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 生成文件全路径
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <param name="id">文档ID</param>
        /// <returns>文件路径</returns>
        protected virtual string GenerateFileFullPath(string id)
        {
            return Path.Combine(GenerateFilePath(),
              GenerateFileName(id));
        }

        /// <summary>
        /// 生成文件路径
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <returns>文件路径</returns>
        protected virtual string GenerateFilePath()
        {
            return Path.Combine(this.MyDirectory, typeof(TEntity).Name);
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <param name="id">文档ID</param>
        /// <returns>文件名</returns>
        protected virtual string GenerateFileName(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            foreach (char c in id)
            {
                if (invalidFileNameChars.Contains(c))
                {
                    throw new FileDatabaseException(
                      string.Format(CultureInfo.InvariantCulture,
                      "The character '{0}' is not a valid file name identifier.", c));
                }
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", id, FileExtension);
        }

        /// <summary>
        /// 将指定的文档对象序列化至字符串
        /// </summary>
        /// <param name="value">指定的文档对象</param>
        /// <returns>文档对象序列化后的字符串</returns>
        protected abstract string Serialize(object value, bool isIndent = false);

        /// <summary>
        /// 将字符串反序列化成文档对象
        /// </summary>
        /// <typeparam name="TEntity">文档类型</typeparam>
        /// <param name="data">字符串</param>
        /// <returns>文档对象</returns>
        protected abstract TEntity Deserialize(string data);

        #endregion
    }
}
