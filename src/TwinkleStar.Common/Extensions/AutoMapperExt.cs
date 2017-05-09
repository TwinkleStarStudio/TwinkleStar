using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinkleStar.Common.Extensions
{
    public static class AutoMapperExt
    {/// <summary>
     ///  类型映射
     /// </summary>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            var config = new MapperConfiguration(cfg => cfg.CreateMap(obj.GetType(), typeof(T)));
            //config.AssertConfigurationIsValid();//←所有属性是否都被映射，如果存在未被映射的属性
            var mapper = config.CreateMapper();
            return mapper.Map<T>(obj);
        }
        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return null;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            //config.AssertConfigurationIsValid();//所有属性是否都被映射，如果存在未被映射的属性
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            foreach (var first in source)
            {
                var type = first.GetType();
                var config = new MapperConfiguration(cfg => cfg.CreateMap(type, typeof(TDestination)));
                //config.AssertConfigurationIsValid();//←所有属性是否都被映射，如果存在未被映射的属性
                var mapper = config.CreateMapper();
                return mapper.Map<List<TDestination>>(source);
            }
            return new List<TDestination>();
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            //IEnumerable<T> 类型需要创建元素的映射
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            //config.AssertConfigurationIsValid();//←所有属性是否都被映射，如果存在未被映射的属性
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }

    }
}
