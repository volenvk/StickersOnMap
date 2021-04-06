using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StickersOnMap.DAL.Interfaces
{
    using Core.Interfaces;
    
    public interface IStateRepository<T> where T : class, IEntity
    {
        IQueryable<T> All();
        T Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate = null);
    }
}