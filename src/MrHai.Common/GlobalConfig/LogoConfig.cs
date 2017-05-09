using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrHai.Core.Data.Repositories;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;

namespace MrHai.Common.GlobalConfig
{
    public class LogoConfig
        : GlobalConfigBase, IGlobalConfig<Logo>
    {
        public LogoConfig(IGlobalConfigRepository repository) : base(repository)
        {
        }

        public Logo GetConfig()
        {
            Logo logoImage = null;
            try
            {
                var config = this.m_repository.GetModel(x => x.ConfigType == this.m_configType).FirstOrDefault();
                if (config == null)
                {
                    return null;
                }

                logoImage = new Logo()
                {
                    ID = config.Id,
                    DeletedTime = config.DeletedTime,
                    Enable = config.Enabled,
                    IsDeleted = config.IsDeleted,
                    LogoSetting = JsonHelper.Deserialize<LogoSetting>(config.Config)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return logoImage;
        }

        public int SaveConfig(Logo config)
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
                Config = JsonHelper.Serialize(config.LogoSetting, false)
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
