using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common;
using TwinkleStar.Common.Others;

namespace MrHai.Core
{
    /// <summary>
    /// 用户相关
    /// </summary>
    public abstract partial class MrHaiService
    {
        public OperationResult Login(string userCode, string password)
        {
            try
            {
                string pwd = MD5Helper.Encrypt(password);

                var user = UserRepository.GetModel(x => x.UserCode.Equals(userCode) && x.Password.Equals(pwd)).FirstOrDefault();
                return new OperationResult(OperationResultType.Success, "", user);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}
