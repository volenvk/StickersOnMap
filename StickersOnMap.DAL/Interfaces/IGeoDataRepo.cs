namespace StickersOnMap.DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Models;

    public interface IGeoDataRepo
    {
        IEnumerable<ModelGeoData> GetActive();
        int Add(ModelGeoData model);
        int Update(ModelGeoData model);
        int Delete(Expression<Func<ModelGeoData, bool>> predicate);
    }
}