using System;
using System.Linq;
using MrHai.Core.Data.Repositories;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;
using System.Collections.Generic;

namespace MrHai.Common.GlobalConfig
{
    public class AboutUsConfig
         : GlobalConfigBase, IGlobalConfig<AboutUs>
    {
        public AboutUsConfig(IGlobalConfigRepository repository)
            : base(repository)
        {
        }

        public AboutUs GetConfig()
        {
            AboutUs aboutUs = null;
            try
            {
                var config = this.m_repository.GetModel(x => x.ConfigType == this.m_configType).FirstOrDefault();
                if (config == null)
                {
                    return null;
                }

                aboutUs = new AboutUs()
                {
                    ID = config.Id,
                    DeletedTime = config.DeletedTime,
                    Enable = config.Enabled,
                    IsDeleted = config.IsDeleted,
                    AboutUsSetting = JsonHelper.Deserialize<IList<AboutUsSetting>>(config.Config)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return aboutUs;
        }

        public int SaveConfig(AboutUs config)
        {
            if (config == null)
            {
                throw new ArgumentNullException();
            }

            MrHai.Core.Models.GlobalConfig conf = new Core.Models.GlobalConfig()
            {
                ConfigType = this.m_configType,
                Enabled = config.Enable,
                DeletedTime = config.DeletedTime,
                IsDeleted = config.IsDeleted,
                Config = JsonHelper.Serialize(config.AboutUsSetting, false)
            };

            if (string.IsNullOrEmpty(config.ID))
            {//insert
                return this.m_repository.Insert(conf);
            }
            else
            {//update
                conf.Id = config.ID;
                return this.m_repository.Update(conf);
            }
        }
    }
}
