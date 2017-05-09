using System;
using MrHai.Core.Data.Repositories;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;
using System.Collections.Generic;
using System.Linq;

namespace MrHai.Common.GlobalConfig
{
    public class FragmentConfig
        : GlobalConfigBase, IGlobalConfig<Fragment>
    {
        public FragmentConfig(IGlobalConfigRepository repository) : base(repository)
        {
        }

        public Fragment GetConfig()
        {
            Fragment fragment = null;

            try
            {
                var config = this.m_repository.GetModel(x => x.ConfigType == this.m_configType).FirstOrDefault();
                if (config == null)
                {
                    return null;
                }

                fragment = new Fragment()
                {
                    ID = config.Id,
                    DeletedTime = config.DeletedTime,
                    Enable = config.Enabled,
                    IsDeleted = config.IsDeleted,
                    Fragments = JsonHelper.Deserialize<IList<FragmentSetting>>(config.Config)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fragment;
        }

        public int SaveConfig(Fragment config)
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
                Config = JsonHelper.Serialize(config.Fragments, false)
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
