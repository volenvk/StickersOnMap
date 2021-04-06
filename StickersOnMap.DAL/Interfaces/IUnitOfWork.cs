using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace StickersOnMap.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        DbSet<T> GetTable<T>() where T : class;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> exp) where T : class;
        void Dispose();
        int Save();
    }
}