using System;
using System.Linq.Expressions;

namespace StickersOnMap.DAL.Repositories
{
    using System.Linq;
    using DbContexts;
    using Models;
    using Interfaces;
    
    public class StickerRepo : IStickerRepo
    {
        private readonly IStateRepository<ModelSticker> _baseStateRepository;
        private readonly IModRepository<ModelSticker> _baseModRepository;
        private readonly IUnitOfWork _uow;
        
        public StickerRepo(StickersDb context)
        {
            _uow = new UnitOfWork(context);
            _baseStateRepository = new BaseStateRepository<ModelSticker>(_uow);
            _baseModRepository = new BaseModRepository<ModelSticker>(_uow);
        }

        public IQueryable<ModelSticker> All()
        {
            return _baseStateRepository.All();
        }

        public bool Any(Expression<Func<ModelSticker, bool>> predicate = null)
        {
            return _baseStateRepository.Any(predicate);
        }

        public IQueryable<ModelSticker> Where(Expression<Func<ModelSticker, bool>> predicate)
        {
            return _baseStateRepository.Where(predicate);
        }

        public ModelSticker Get(Expression<Func<ModelSticker, bool>> predicate)
        {
            return _baseStateRepository.Get(predicate);
        }

        public int Add(ModelSticker model)
        {
            return _baseModRepository.Add(model);
        }
        
        public int Update(ModelSticker model)
        {
            return _baseModRepository.Update(model);
        }

        public int Delete(Expression<Func<ModelSticker, bool>> predicate)
        {
            return _baseModRepository.Delete(predicate);
        }
    }
}