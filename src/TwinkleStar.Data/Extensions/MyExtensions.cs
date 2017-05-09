using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace TwinkleStar.Data.Extensions
{
    public static class MyExtensions
    {
        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EntitySetBase GetEntitySet(this ObjectContext context, Type type)
        {
            string className = type.Name;

            var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);

            EntitySetBase entitySet = (from meta in container.BaseEntitySets
                                    where meta.ElementType.Name == className
                                    select meta).First();

            return entitySet;
        }
    }
}
