using MrHai.Core.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;

namespace MrHai.Common.GlobalConfig
{
    public class FriendlyLinksConfig : GlobalConfigBase, IGlobalConfig<FriendlyLinks>
    {
        public FriendlyLinksConfig(IGlobalConfigRepository repository):base(repository)
        {
        }

        public FriendlyLinks GetConfig()
        {
            FriendlyLinks links = null;

            try
            {
                var config = this.m_repository.GetModel(x => x.ConfigType == this.m_configType).FirstOrDefault();
                if (config == null)
                {
                    return null;
                }

                links = new FriendlyLinks()
                {
                    ID = config.Id,
                    DeletedTime = config.DeletedTime,
                    Enable = config.Enabled,
                    IsDeleted = config.IsDeleted,
                    Links = JsonHelper.Deserialize<IList<FriendlyLink>>(config.Config)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return links;
        }

        public int SaveConfig(FriendlyLinks config)
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
                Config = JsonHelper.Serialize(config.Links, false)
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
