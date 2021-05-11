using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NLog;

namespace StickersOnMap.DAL.Repositories
{
    using Interfaces;
    using Core.Interfaces;

    public class StateRepository<T> : IStateRepository<T> where T : class, IEntity
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _uow; 

        public StateRepository(IRepositoryConfig config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _logger = LogManager.GetCurrentClassLogger();
            _uow = config.CreateSingletonUnitOfWork();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _uow.GetQueryable<T>().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                if (predicate != null)
                    ex.Data.Add("params:", predicate.Body.ToString());
                _logger.Error(ex, $"[{nameof(GetFirstOrDefault)}] " + ex.Message);
                return null;
            }
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _uow.GetQueryable<T>().Where(predicate).ToList();
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
                return predicate == null ? _uow.GetQueryable<T>().Any() : _uow.GetQueryable<T>().Any(predicate);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(Any)}] " + ex.Message);
                return false;
            }
        }
    }
}