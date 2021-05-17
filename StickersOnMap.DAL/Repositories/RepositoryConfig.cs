namespace StickersOnMap.DAL.Repositories
{
    using System;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public sealed class RepositoryConfig : IRepositoryConfig
    {
        private readonly DbContext _context;

        public RepositoryConfig(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Func<IRepository<T>> GetRepository<T>() where T : class
        {
            return () => new Repository<T>(_context);
        }
    }
}