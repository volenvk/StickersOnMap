using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StickersOnMap.DAL.Interfaces
{
    using Core.Interfaces;
    
    public interface IStateRepository<T> where T : class, IEntity
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate = null);
    }
}