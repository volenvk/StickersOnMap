namespace StickersOnMap.DAL.Repositories
{
    using System;
    using System.Linq;
    using DbContexts;
    using Interfaces;

    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _context;

        public Repository(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<T> GetQueryable() => _context.Set<T>().AsQueryable();
        
        public void Insert(T model) => _context.Add(model);

        public void Update(T model) => _context.Update(model);

        public void Delete(T model) => _context.Remove(model);

        public int Save() => _context.SaveChanges();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}