namespace StickersOnMap.DAL.Interfaces
{
    public interface IRepositoryConfig
    {
        IUnitOfWork CreateSingletonUnitOfWork();
    }
}