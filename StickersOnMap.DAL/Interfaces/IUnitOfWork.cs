namespace StickersOnMap.DAL.Interfaces
{
    using System.Linq;

    public interface IUnitOfWork
    {
        IQueryable<T> GetQueryable<T>() where T : class;
        void Insert<T>(T model) where T : class;
        void Update<T>(T model) where T : class;
        void Delete<T>(T model) where T : class;
        int Save();
    }
}