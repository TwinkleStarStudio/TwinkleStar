using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using TwinkleStar.Data;
using MrHai.Core.Models;
using TwinkleStar.Service.Migrations;
using MrHai.Core.Models.Enums;
using TwinkleStar.Common;

namespace MrHai.Core.Data.Migrations
{
    public class MrHaiConfiguration : Configuration
    {
        public MrHaiConfiguration():base()
        {
        }

        protected override void Seed(EFDbContext context)
        {
            IList<Category> categoryList = new List<Category>();

            for (int i = 0; i < GlobalConstants.MainNavigation.Length; i++)
            {
                Category category = new Category()
                {
                    Id = (i + 1).ToString(),
                    CategoryType = NavigationType.MainNavigation,
                    Enabled = true,
                    Icon = "",
                    CreateTime = DateTime.Now,
                    DeletedTime = null,
                    IsDeleted = false,
                    Level = 0,
                    Name = GlobalConstants.MainNavigation[i],
                    OrderNum = GlobalConstants.MainNavigation.Length - i,
                    ParentId = null
                };

                if (GlobalConstants.MainNavigation[i].Equals("contact"))
                {
                    category.Url = GlobalConstants.ContactUsUrl;
                }
                else if (GlobalConstants.MainNavigation[i].Equals("artist cv"))
                {
                    category.Url = GlobalConstants.ArtistUrl;
                }

                categoryList.Add(category);
            }

            DbSet<Category> categorySet = context.Set<Category>();
            categorySet.AddOrUpdate(m => new { m.Id }, categoryList.ToArray());

            DbSet<User> userSet = context.Set<User>();
            userSet.AddOrUpdate(m => new { m.UserId }, new User() { UserCode = "admin", Password = MD5Helper.Encrypt("123456"), UserName = "Admin" });
        }
    }
}
