namespace StickersOnMap.DAL.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using NLog;
    using Interfaces;
    using System.Threading.Tasks;
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;

        public UnitOfWork(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = LogManager.GetCurrentClassLogger();
        }

        public DbSet<T> GetTable<T>() where T : class => _context.Set<T>();
        
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> exp) where T : class
        {
            try
            {
                return _context.Set<T>().AsQueryable().Where(exp);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }

        public void Dispose() => _context.Dispose();

        public int Save() => _context.SaveChanges();

        public Task<int> SaveAsync() => _context.SaveChangesAsync();
    }
}