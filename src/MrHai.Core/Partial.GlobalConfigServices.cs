using MrHai.Common.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common.Others;

namespace MrHai.Core
{
    /// <summary>
    /// Global相关
    /// </summary>
    public abstract partial class MrHaiService
    {
        public virtual OperationResult GetFriendlyLinks()
        {
            FriendlyLinksConfig config = new FriendlyLinksConfig(GlobalConfigRepository);

            var conf = config.GetConfig();

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }

        public virtual int SaveFriendlyLinks(FriendlyLinks friendlyLinks)
        {
            FriendlyLinksConfig config = new FriendlyLinksConfig(GlobalConfigRepository);

            return config.SaveConfig(friendlyLinks);
        }

        /// <summary>
        /// 获取关于我们
        /// </summary>
        /// <returns></returns>
        public virtual OperationResult GetAboutUs()
        {
            AboutUsConfig config = new AboutUsConfig(GlobalConfigRepository);

            var conf = config.GetConfig();

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }
        /// <summary>
        /// 保存关于我们
        /// </summary>
        /// <param name="aboutUs"></param>
        public virtual void SaveAboutUs(AboutUs aboutUs)
        {
            AboutUsConfig config = new AboutUsConfig(GlobalConfigRepository);

            config.SaveConfig(aboutUs);
        }

        /// <summary>
        /// 获取碎片配置
        /// </summary>
        /// <returns></returns>
        public virtual OperationResult GetFragment()
        {
            FragmentConfig config = new FragmentConfig(GlobalConfigRepository);

            var conf = config.GetConfig();

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }/// <summary>
         /// 获取碎片配置
         /// </summary>
         /// <returns></returns>
        public virtual OperationResult GetFragment(int num)
        {
            FragmentConfig config = new FragmentConfig(GlobalConfigRepository);

            var conf = config.GetConfig()?.Fragments?.FirstOrDefault(p=>p.SerialNumber ==num);

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }
        /// <summary>
        /// 保存碎片配置
        /// </summary>
        /// <param name="fragment"></param>
        public virtual void SaveFragment(Fragment fragment)
        {
            FragmentConfig config = new FragmentConfig(GlobalConfigRepository); ;
            config.SaveConfig(fragment);
        }

        /// <summary>
        /// 获取logo
        /// </summary>
        /// <returns></returns>
        public virtual OperationResult GetLogo()
        {
            LogoConfig config = new LogoConfig(GlobalConfigRepository);

            var conf = config.GetConfig();

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }
        /// <summary>
        /// 保存logo
        /// </summary>
        /// <param name="logo"></param>
        public virtual void SaveLogo(Logo logo)
        {
            LogoConfig config = new LogoConfig(GlobalConfigRepository);
            config.SaveConfig(logo);
        }

        /// <summary>
        /// 获取官网链接
        /// </summary>
        /// <returns></returns>
        public virtual OperationResult GetOfficialWebsite()
        {
            OfficialWebsiteConfig config = new OfficialWebsiteConfig(GlobalConfigRepository);

            var conf = config.GetConfig();

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }
        /// <summary>
        /// 保存官网链接
        /// </summary>
        /// <param name="officialWebsite"></param>
        public virtual void SaveOfficialWebsite(OfficialWebsite officialWebsite)
        {
            OfficialWebsiteConfig config = new OfficialWebsiteConfig(GlobalConfigRepository);

            config.SaveConfig(officialWebsite);
        }

        /// <summary>
        /// 获取最新作品
        /// </summary>
        /// <returns></returns>
        public virtual OperationResult GetNewWorks()
        {
            NewWorksConfig config = new NewWorksConfig(GlobalConfigRepository);

            var conf = config.GetConfig();

            if (conf == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定配置不存在。");
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "查询成功", conf);
            }
        }
        /// <summary>
        /// 保存最新作品
        /// </summary>
        /// <param name="officialWebsite"></param>
        public virtual void SaveNewWorks(NewWorks newWorks)
        {
            NewWorksConfig config = new NewWorksConfig(GlobalConfigRepository);

            config.SaveConfig(newWorks);
        }
    }
}
