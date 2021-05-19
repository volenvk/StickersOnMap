namespace StickersOnMap.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Interfaces;
    using DAL.DbContexts;
    using DAL.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using NSubstitute;

    internal class FakeStickersDb<TEntity> : IStickersDb where TEntity : class, IEntity
    {
        public DbSet<ModelSticker> Stickers { get; }

        private DbSet<TEntity> Db { get; }
        private ICollection<TEntity> _source;

        public FakeStickersDb()
        {
            Db = Substitute.For<DbSet<TEntity>>();
        }

        public void SetCollection(IQueryable<TEntity> source)
        {
            Db.AsQueryable().Returns(source);
            _source = source.ToList();
        }

        public DbSet<T> Set<T>() where T : class
        {
            return Db as DbSet<T>;
        }

        public EntityEntry<T> Add<T>(T model) where T : class
        {
            if (model is TEntity entity)
            {
                _source.Add(entity);
                Db.AsQueryable().Returns(_source.AsQueryable());
                return Db.Add(entity) as EntityEntry<T>;
            }

            return null;
        }

        public EntityEntry<T> Update<T>(T model) where T : class
        {
            if (model is TEntity entity)
            {
                var current = _source.FirstOrDefault(m => m.Id == entity.Id);
                if (current != null)
                {
                    _source.Remove(current);
                    _source.Add(entity);
                    Db.AsQueryable().Returns(_source.AsQueryable());
                    return Db.Update(entity) as EntityEntry<T>;   
                }
            }

            return null;
        }

        public EntityEntry<T> Remove<T>(T model) where T : class
        {
            if (model is TEntity entity)
            {
                _source.Remove(entity);
                Db.AsQueryable().Returns(_source.AsQueryable());
                return Db.Remove(entity) as EntityEntry<T>;
            }

            return null;
        }

        public int SaveChanges()
        {
            return 1;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}