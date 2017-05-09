
using System;
using System.ComponentModel;
using TwinkleStar.Data;

namespace TwinkleStar.Demo.Core.Models
{
    /// <summary>
    ///     ʵ���ࡪ���û���չ��Ϣ
    /// </summary>
    [Description("�û���չ��Ϣ")]
    public class MemberExtend : EntityBase
    {
        /// <summary>
        /// ��ʼ��һ�� �û���չʵ���� ����ʵ��
        /// </summary>
        public MemberExtend()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Tel { get; set; }

        public MemberAddress Address { get; set; }

        public virtual Member Member { get; set; }
    }
}