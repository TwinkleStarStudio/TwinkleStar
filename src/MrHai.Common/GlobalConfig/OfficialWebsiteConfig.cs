using System;
using MrHai.Core.Data.Repositories;
using System.Linq;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;

namespace MrHai.Common.GlobalConfig
{
    public class OfficialWebsiteConfig :GlobalConfigBase, IGlobalConfig<OfficialWebsite>
    {
        public OfficialWebsiteConfig(IGlobalConfigRepository repository)
            :base(repository)
        {
        }

        public OfficialWebsite GetConfig()
        {
            OfficialWebsite webSite = null;

            try
            {
                var config = this.m_repository.GetModel(x => x.ConfigType == this.m_configType).FirstOrDefault();
                if (config == null)
                {
                    return null;
                }

                webSite = new OfficialWebsite()
                {
                    ID = config.Id,
                    DeletedTime = config.DeletedTime,
                    Enable = config.Enabled,
                    IsDeleted = config.IsDeleted,
                    OfficialWebsiteSetting = JsonHelper.Deserialize<OfficialWebsiteSetting>(config.Config)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return webSite;
        }

        public int SaveConfig(OfficialWebsite config)
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
                Config = JsonHelper.Serialize(config.OfficialWebsiteSetting, false)
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
