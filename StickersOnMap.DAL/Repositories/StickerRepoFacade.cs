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
    
    public class StickerRepoFacade : IStickerRepoFacade
    {
        private readonly IRepositoryFactory<ModelSticker> _repositoryFactory;
        
        public StickerRepoFacade(StickersDb context, IMapper mapper)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = mapper ?? throw new ArgumentNullException(nameof(mapper));
            var config = new RepositoryConfig(context);
            _repositoryFactory = new RepositoryFactory<ModelSticker>(config, mapper);
        }

        public bool Any(Expression<Func<ModelSticker, bool>> predicate = null)
        {
            var repo = _repositoryFactory.CreateSingletonStateRepo();
            return repo.Any(predicate);
        }

        public PagedList<TD> GetPages<TD>(TableFilter filter) where TD : class, new()
        {
            var repo = _repositoryFactory.CreateSingletonFilterRepo();
            return repo.GetPages<TD>(filter);
        }
        
        public IEnumerable<ModelSticker> Where(Expression<Func<ModelSticker, bool>> predicate)
        {
            var repo = _repositoryFactory.CreateSingletonStateRepo();
            return repo.Where(predicate);
        }

        public ModelSticker GetFirstOrDefault(Expression<Func<ModelSticker, bool>> predicate)
        {
            var repo = _repositoryFactory.CreateSingletonStateRepo();
            return repo.GetFirstOrDefault(predicate);
        }

        public int Add(ModelSticker model)
        {
            var repo = _repositoryFactory.CreateSingletonModeRepo();
            return repo.Add(model);
        }
        
        public int Update(ModelSticker model)
        {
            var repo = _repositoryFactory.CreateSingletonModeRepo();
            return repo.Update(model);
        }

        public int Delete(Expression<Func<ModelSticker, bool>> predicate)
        {
            var repo = _repositoryFactory.CreateSingletonModeRepo();
            return repo.Delete(predicate);
        }
    }
}