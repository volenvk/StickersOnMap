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
        private readonly Func<IRepository<T>> _repository; 

        public StateRepository(IRepositoryConfig config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _logger = LogManager.GetCurrentClassLogger();
            _repository = config.GetRepository<T>();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            try
            {
                using var repo = _repository();
                return repo.GetQueryable().FirstOrDefault(predicate);
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
                using var repo = _repository();
                return repo.GetQueryable().Where(predicate).ToList();
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
                using var repo = _repository();
                return predicate == null ? repo.GetQueryable().Any() : repo.GetQueryable().Any(predicate);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(Any)}] " + ex.Message);
                return false;
            }
        }
    }
}