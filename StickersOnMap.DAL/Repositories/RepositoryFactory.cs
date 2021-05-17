using System;
using AutoMapper;

namespace StickersOnMap.DAL.Repositories
{
    using Core.Interfaces;
    using Interfaces;
    
    public sealed class RepositoryFactory<T> : IRepositoryFactory<T> where T : class, IEntity
    {
        private readonly Lazy<IStateRepository<T>> _stateRepoInstance;
        private readonly Lazy<IModeRepository<T>> _modeRepoInstance;
        private readonly Lazy<IFilterRepositoryByMap<T>> _filterRepoInstance;

        public RepositoryFactory(IRepositoryConfig config, IMapper mapper)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _ = mapper ?? throw new ArgumentNullException(nameof(mapper));
            
            _stateRepoInstance = new Lazy<IStateRepository<T>>(() => new StateRepository<T>(config));
            _modeRepoInstance = new Lazy<IModeRepository<T>>(() => new ModeRepository<T>(config));
            _filterRepoInstance = new Lazy<IFilterRepositoryByMap<T>>(()=> new FilterRepositoryByMap<T>(config, mapper));
        }

        public IStateRepository<T> CreateLazyStateRepo() => _stateRepoInstance.Value;
        
        public IModeRepository<T> CreateLazyModeRepo() => _modeRepoInstance.Value;
        
        public IFilterRepositoryByMap<T> CreateLazyFilterRepo() => _filterRepoInstance.Value;
    }
}