using TwinkleStar.Data;

namespace TwinkleStar.Demo.Core.Models
{
    /// <summary>
    ///     ��¼��Ϣ��
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        ///     ��ȡ������ ��¼�˺�
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     ��ȡ������ ��¼����
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     ��ȡ������ IP��ַ
        /// </summary>
        public string IpAddress { get; set; }
    }
}