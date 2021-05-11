using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NLog;

namespace StickersOnMap.DAL.Repositories
{
    using System.Linq;
    using Interfaces;
    using Core.Interfaces;
    
    public class ModeRepository<T> : IModeRepository<T> where T : class, IEntity
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _uow;

        public ModeRepository(IRepositoryConfig config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _logger = LogManager.GetCurrentClassLogger();
            _uow = config.CreateSingletonUnitOfWork();
        }
        
        public int Add(T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                model.CreateDate = DateTime.Now;
                _uow.Insert(model);
                return _uow.Save();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(Add)}] " + ex.Message);
                return 0;
            }
        }
        
        public int AddRange(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            try
            {
                foreach (var model in source)
                {
                    model.CreateDate = DateTime.Now;
                    _uow.Insert(model);
                }

                return _uow.Save();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(AddRange)}] " + ex.Message);
                return 0;
            }
        }

        public int Update(T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                if (!_uow.GetQueryable<T>().Any(dt => dt.Id == model.Id)) 
                    return 0;
                
                model.CreateDate = DateTime.Now;
                _uow.Update(model);
                return _uow.Save();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[{nameof(Update)}] " + ex.Message);
                return 0;
            }
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var model = _uow.GetQueryable<T>().FirstOrDefault(predicate);
                if (model == null)
                    throw new ArgumentNullException(nameof(model));
                
                _uow.Delete(model);
                return _uow.Save();
            }
            catch (Exception ex)
            {
                if (predicate != null)
                    ex.Data.Add("params:", predicate.Body.ToString());
                _logger.Error(ex, $"[{nameof(Delete)}] {ex.Message}");
                return 0;
            }
        }
    }
}