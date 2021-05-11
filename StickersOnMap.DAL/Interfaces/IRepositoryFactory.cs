namespace StickersOnMap.DAL.Interfaces
{
    using Core.Interfaces;

    public interface IRepositoryFactory<T> where T : class, IEntity
    {
        IStateRepository<T> CreateSingletonStateRepo();
        IModeRepository<T> CreateSingletonModeRepo();
        IFilterRepositoryByMap<T> CreateSingletonFilterRepo();
    }
}