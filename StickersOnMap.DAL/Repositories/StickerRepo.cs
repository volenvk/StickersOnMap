using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoMapper;

namespace StickersOnMap.DAL.Repositories
{
    using Core.Infrastructure.Filteres;
    using Core.Infrastructure.Pages;
    using DbContexts;
    using Models;
    using Interfaces;
    
    public class StickerRepo : IStickerRepo
    {
        private readonly IRepositoryFactory<ModelSticker> _repositorySticker;
        
        public StickerRepo(IStickersDb context, IMapper mapper)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = mapper ?? throw new ArgumentNullException(nameof(mapper));
            var config = new RepositoryConfig(context);
            _repositorySticker = new RepositoryFactory<ModelSticker>(config, mapper);
        }

        public PagedList<T> GetPages<T>(TableFilter filter) where T : class, new()
        {
            var repo = _repositorySticker.CreateLazyFilterRepo();
            return repo.GetPages<T>(filter);
        }
        
        public bool Any(Expression<Func<ModelSticker, bool>> predicate = null)
        {
            var repo = _repositorySticker.CreateLazyStateRepo();
            return repo.Any(predicate);
        }
        
        public IEnumerable<ModelSticker> Where(Expression<Func<ModelSticker, bool>> predicate)
        {
            var repo = _repositorySticker.CreateLazyStateRepo();
            return repo.Where(predicate);
        }

        public ModelSticker GetFirstOrDefault(Expression<Func<ModelSticker, bool>> predicate)
        {
            var repo = _repositorySticker.CreateLazyStateRepo();
            return repo.GetFirstOrDefault(predicate);
        }

        public int Add(ModelSticker model)
        {
            var repo = _repositorySticker.CreateLazyModeRepo();
            return repo.Add(model);
        }
        
        public int Update(ModelSticker model)
        {
            var repo = _repositorySticker.CreateLazyModeRepo();
            return repo.Update(model);
        }

        public int Delete(Expression<Func<ModelSticker, bool>> predicate)
        {
            var repo = _repositorySticker.CreateLazyModeRepo();
            return repo.Delete(predicate);
        }
    }
}