using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TwinkleStar.Data;

namespace TwinkleStar.Demo.Core.Models
{
    /// <summary>
    ///     ʵ���ࡪ����ɫ��Ϣ
    /// </summary>
    [Description("��ɫ��Ϣ")]
    public class Role : EntityBase
    {
        public Role()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// ��ȡ������ ��ɫ����
        /// </summary>
        public RoleType RoleType
        {
            get { return (RoleType)RoleTypeNum; }
            set { RoleTypeNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ ��ɫ���͵���ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int RoleTypeNum { get; set; }

        /// <summary>
        ///     ��ȡ������ ӵ�д˽�ɫ���û���Ϣ����
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }
    }
}
