using System;

namespace TwinkleStar.Data.LocalFileDB
{
    /// <summary>
    /// 文件数据库异常
    /// </summary>
    public class FileDatabaseException : Exception
    {
        public FileDatabaseException(string message)
            : base(message)
        { }

        public FileDatabaseException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
