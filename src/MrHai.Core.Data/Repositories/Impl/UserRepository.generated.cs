﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//	   如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类进行实现。
// </auto-generated>
//
// <copyright file="UserConfiguration.generated.cs">
//		Copyright(c)2017 TwinkleStar.All rights reserved.
//		开发组织：ToToStarStudio@中国
//		公司网站：
//		所属工程：
//		生成时间：2017-04-15 17:00
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;

using TwinkleStar.Data;
using MrHai.Core.Models;


namespace MrHai.Core.Data.Repositories.Impl
{
	/// <summary>
    ///   仓储操作层实现——用户
    /// </summary>
    [Export(typeof(IUserRepository))]
    public partial class UserRepository : DbContextRepository<User>, IUserRepository
    { }
}
