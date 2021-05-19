namespace StickersOnMap.DAL.Repositories
{
    using System;
    using DbContexts;
    using Interfaces;

    public sealed class RepositoryConfig : IRepositoryConfig
    {
        private readonly IDbContext _context;

        public RepositoryConfig(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Func<IRepository<T>> GetRepository<T>() where T : class
        {
            return () => new Repository<T>(_context);
        }
    }
}