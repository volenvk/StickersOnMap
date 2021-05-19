namespace StickersOnMap.DAL.DbContexts
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public interface IDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Add<T>(T model) where T : class;
        EntityEntry<T> Update<T>(T model) where T : class;
        EntityEntry<T> Remove<T>(T model) where T : class;
        int SaveChanges();
    }
}