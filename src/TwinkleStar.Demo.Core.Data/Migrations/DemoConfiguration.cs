using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using TwinkleStar.Data;
using TwinkleStar.Demo.Core.Models;
using TwinkleStar.Service.Migrations;

namespace TwinkleStar.Demo.Core.Data.Migrations
{
    public class DemoConfiguration : Configuration
    {
        public DemoConfiguration():base()
        {
        }

        protected override void Seed(EFDbContext context)
        {
        //    List<Role> roles = new List<Role>
        //    {
        //        new Role{ Name = "系统管理", Description = "系统管理角色，拥有整个系统的管理权限。", RoleType = RoleType.Admin},
        //        new Role{ Name = "蓝钻", Description = "蓝钻会员角色", RoleType = RoleType.User},
        //        new Role{ Name = "红钻", Description = "红钻会员角色", RoleType = RoleType.User},
        //        new Role{ Name = "黄钻", Description = "黄钻会员角色", RoleType = RoleType.User},
        //        new Role{ Name = "绿钻", Description = "绿钻会员角色", RoleType = RoleType.User}
        //    };
        //    DbSet<Role> roleSet = context.Set<Role>();
        //    roleSet.AddOrUpdate(m => new { m.Name }, roles.ToArray());
        //    context.SaveChanges();

        //    List<Member> members = new List<Member>
        //    {
        //        new Member { UserName = "admin", Password = "123456", Email = "admin@gmfcn.net", NickName = "管理员" },
        //        new Member { UserName = "gmfcn", Password = "123456", Email = "mf.guo@qq.com", NickName = "郭明锋" }
        //    };

        //    for (int i = 0; i < 100; i++)
        //    {
        //        Random rnd = new Random((int)DateTime.Now.Ticks + i);
        //        Member member = new Member
        //        {
        //            UserName = "userName" + i,
        //            Password = "123456",
        //            Email = "userName" + i + "@gmfcn.net",
        //            NickName = "用户" + i
        //        };
        //        var roleArray = roleSet.ToArray();
        //        member.Roles.Add(roleArray[rnd.Next(0, roleArray.Length)]);
        //        if (rnd.NextDouble() > 0.5)
        //        {
        //            member.Roles.Add(roleArray[rnd.Next(1, roleArray.Length)]);
        //        }
        //        members.Add(member);
        //    }
        //    DbSet<Member> memberSet = context.Set<Member>();
        //    memberSet.AddOrUpdate(m => new { m.UserName }, members.ToArray());
        }
    }
}
