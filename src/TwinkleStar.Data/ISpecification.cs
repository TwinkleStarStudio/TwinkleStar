using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TwinkleStar.Data
{
    /// <summary>
    /// 基础规约
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        bool IsSatisfiedBy(TEntity entity);

        Expression<Func<TEntity, bool>> Expression { get; }
    }
}
