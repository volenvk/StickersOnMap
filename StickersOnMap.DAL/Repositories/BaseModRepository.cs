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

    public class BaseModRepository<T> : IModRepository<T> where T : class, IEntity
    {
        private readonly ILogger _logger;
        private readonly DbSet<T> _db;
        private readonly IUnitOfWork _uow; 

        public BaseModRepository(IUnitOfWork uow)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _uow = uow;
            _db = uow.GetTable<T>();
        }
        
        public int Add(T model)
        {
            if (model == null)
                return 0;

            try
            {
                model.CreateDate = DateTime.Now;
                _db.Add(model);
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
            if (source?.Any() != true)
                return 0;

            try
            {
                foreach (var model in source)
                {
                    model.CreateDate = DateTime.Now;
                }
                _db.AddRange(source);
                
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
                return 0;

            try
            {
                if (!_db.Any(dt => dt.Id == model.Id)) 
                    return 0;
                
                model.CreateDate = DateTime.Now;
                _db.Update(model);
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
                var modelDb = _db.AsQueryable().FirstOrDefault(predicate);
                if (modelDb == null) 
                    return 0;
                
                _db.Remove(modelDb);
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