namespace StickersOnMap.DAL.Interfaces
{
    using System;
    using System.Linq;

    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetQueryable();
        void Insert(T model);
        void Update(T model);
        void Delete(T model);
        int Save();
    }
}