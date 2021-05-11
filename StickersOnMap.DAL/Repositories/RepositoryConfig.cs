namespace StickersOnMap.DAL.Repositories
{
    using System;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public sealed class RepositoryConfig : IRepositoryConfig
    {
        private static Lazy<IUnitOfWork> _uowInstance;

        public RepositoryConfig(DbContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _uowInstance = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
        }

        public IUnitOfWork CreateSingletonUnitOfWork() => _uowInstance.Value;
    }
}