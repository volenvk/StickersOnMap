namespace StickersOnMap.DAL.Interfaces
{
    using Core.Interfaces;

    public interface IRepositoryFactory<T> where T : class, IEntity
    {
        IStateRepository<T> CreateLazyStateRepo();
        IModeRepository<T> CreateLazyModeRepo();
        IFilterRepositoryByMap<T> CreateLazyFilterRepo();
    }
}