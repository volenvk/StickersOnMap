using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StickersOnMap.DAL.Interfaces
{
    using Core.Interfaces;
    
    public interface IModRepository<T> where T : class, IEntity
    {
        int Add(T model);
        int AddRange(IEnumerable<T> source);
        int Update(T model);
        int Delete(Expression<Func<T, bool>> predicate);
    }
}