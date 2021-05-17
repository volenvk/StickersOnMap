namespace StickersOnMap.DAL.Interfaces
{
    using System;

    public interface IRepositoryConfig
    {
        Func<IRepository<T>> GetRepository<T>() where T : class;
    }
}