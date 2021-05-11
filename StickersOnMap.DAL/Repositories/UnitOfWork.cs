namespace StickersOnMap.DAL.Repositories
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Interfaces;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<T> GetQueryable<T>() where T : class => _context.Set<T>().AsQueryable();
        
        public void Insert<T>(T model) where T : class => _context.Add(model);

        public void Update<T>(T model) where T : class => _context.Update(model);

        public void Delete<T>(T model) where T : class => _context.Remove(model);

        public int Save() => _context.SaveChanges();
    }
}