using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Common
{
    /// <summary>
    /// 枚举描述
    /// </summary>
    public class RemarkAttribute : Attribute
    {
        private string m_remark;
        public RemarkAttribute(string remark)
        {
            m_remark = remark;
        }

        public string Remark
        {
            get
            {
                return this.m_remark;
            }
        }
    }

    public static class RemarkExtension
    {
        public static string GetRemark(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo field = type.GetField(value.ToString());
            RemarkAttribute remarkAttribute = (RemarkAttribute)field.GetCustomAttribute(typeof(RemarkAttribute));
            return remarkAttribute.Remark;
        }
    }
}
