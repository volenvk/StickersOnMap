using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace StickersOnMap.DAL.Repositories
{
    using Interfaces;
    using Core.Interfaces;
    
    public class BaseStateRepository<T> : IStateRepository<T> where T : class, IEntity
    {
        private readonly ILogger _logger;
        private readonly DbSet<T> _db;
        private readonly IUnitOfWork _uow; 

        public BaseStateRepository(IUnitOfWork uow)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _uow = uow;
            _db = uow.GetTable<T>();
        }

        public IQueryable<T> All()
        {
            try
            {
                return _db.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(All)}] " + ex.Message);
                return null;
            }
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _db.AsQueryable().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                if (predicate != null)
                    ex.Data.Add("params:", predicate.Body.ToString());
                _logger.Error(ex, $"[{nameof(Get)}] " + ex.Message);
                return null;
            }
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _uow.Query(predicate);
            }
            catch (Exception ex)
            {
                if (predicate != null)
                    ex.Data.Add("params:", predicate.Body.ToString());
                _logger.Error(ex, $"[{nameof(Where)}] " + ex.Message);
                return null;
            }
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                return predicate == null ? _db.Any() : _db.Any(predicate);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(Any)}] " + ex.Message);
                return false;
            }
        }
    }
}