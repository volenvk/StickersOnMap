namespace StickersOnMap.DAL.Interfaces
{
    using System.Collections.Generic;
    using Core.Infrastructure.Filteres;
    using Core.Infrastructure.Pages;
    using Core.Interfaces;

    public interface IFilterRepositoryByMap<T> where T : class, IEntity
    {
        IEnumerable<TD> FilterBy<TD>(TableFilter filter) where TD : class, new();
        PagedList<TD> GetPages<TD>(TableFilter filter) where TD : class, new();
    }
}