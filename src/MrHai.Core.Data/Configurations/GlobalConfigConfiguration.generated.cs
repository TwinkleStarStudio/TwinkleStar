﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//		如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类实现 GlobalConfigConfigurationAppend 分部方法。
// </auto-generated>
//
// <copyright file="GlobalConfigConfiguration.generated.cs">
//		Copyright(c)2017 TwinkleStar.All rights reserved.
//		开发组织：ToToStarStudio@中国
//		公司网站：
//		所属工程：
//		生成时间：2017-04-15 17:00
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using TwinkleStar.Data;
using MrHai.Core.Models;


namespace MrHai.Core.Data.Configurations
{
    /// <summary>
    /// 实体类-数据表映射——全局配置
    /// </summary>    
	internal partial class GlobalConfigConfiguration : EntityTypeConfiguration<GlobalConfig>, IEntityMapper
    {
        /// <summary>
        /// 实体类-数据表映射构造函数——全局配置
        /// </summary>
        public GlobalConfigConfiguration()
        {
			GlobalConfigConfigurationAppend();
        }
		
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void GlobalConfigConfigurationAppend();
		
        /// <summary>
        /// 将当前实体映射对象注册到当前数据访问上下文实体映射配置注册器中
        /// </summary>
        /// <param name="configurations">实体映射配置注册器</param>
        public void RegistTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
