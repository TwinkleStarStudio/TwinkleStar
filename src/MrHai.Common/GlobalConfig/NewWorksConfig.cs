using System;
using MrHai.Core.Data.Repositories;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;
using System.Collections.Generic;
using System.Linq;

namespace MrHai.Common.GlobalConfig
{
    public class NewWorksConfig
         : GlobalConfigBase, IGlobalConfig<NewWorks>
    {
        public NewWorksConfig(IGlobalConfigRepository repository) : base(repository)
        {
        }

        public NewWorks GetConfig()
        {
            NewWorks newWorks = null;

            try
            {
                var config = this.m_repository.GetModel(x => x.ConfigType == this.m_configType).FirstOrDefault();
                if (config == null)
                {
                    return null;
                }

                newWorks = new NewWorks()
                {
                    ID = config.Id,
                    DeletedTime = config.DeletedTime,
                    Enable = config.Enabled,
                    IsDeleted = config.IsDeleted,
                    NewWorksList = JsonHelper.Deserialize<IList<NewWorksSetting>>(config.Config)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return newWorks;
        }

        public int SaveConfig(NewWorks config)
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
                Config = JsonHelper.Serialize(config.NewWorksList, false)
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
