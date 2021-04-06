namespace StickersOnMap.DAL.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using DbContexts;
    using Interfaces;
    using Models;

    public class GeoDataRepo : IGeoDataRepo
    {
        private readonly IStateRepository<ModelGeoData> _baseStateRepository;
        private readonly IModRepository<ModelGeoData> _baseModRepository;
        private readonly IUnitOfWork _uow;

        public GeoDataRepo(StickersDb context)
        {
            _uow = new UnitOfWork(context);
            _baseStateRepository = new BaseStateRepository<ModelGeoData>(_uow);
            _baseModRepository = new BaseModRepository<ModelGeoData>(_uow);
        }
        
        public IEnumerable<ModelGeoData> GetActive()
        {
            return _baseStateRepository.Where(t => t.Active).ToArray();
        }

        public int Add(ModelGeoData model)
        {
            return _baseModRepository.Add(model);
        }

        public int Update(ModelGeoData model)
        {
            return _baseModRepository.Update(model);
        }

        public int Delete(Expression<Func<ModelGeoData, bool>> predicate)
        {
            return _baseModRepository.Delete(predicate);
        }
    }
}