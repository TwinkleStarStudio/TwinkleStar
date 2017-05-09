using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TwinkleStar.Data;

namespace TwinkleStar.Demo.Core.Models
{
    /// <summary>
    /// ʵ���ࡪ����¼��¼��Ϣ
    /// </summary>
    [Description("��¼��¼��Ϣ")]
    public class LoginLog : EntityBase
    {
        /// <summary>
        /// ��ʼ��һ�� ��¼��¼ʵ���� ����ʵ��
        /// </summary>
        public LoginLog()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(15)]
        public string IpAddress { get; set; }

        /// <summary>
        /// ��ȡ������ �����û���Ϣ
        /// </summary>
        public virtual Member Member { get; set; }
    }
}