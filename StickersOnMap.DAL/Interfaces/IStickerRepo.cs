using System;
using System.Linq.Expressions;

namespace StickersOnMap.DAL.Interfaces
{
    using System.Collections.Generic;
    using Core.Infrastructure.Filteres;
    using Core.Infrastructure.Pages;
    using Models;
    
    public interface IStickerRepo
    {
        bool Any(Expression<Func<ModelSticker, bool>> predicate = null);
        PagedList<TD> GetPages<TD>(TableFilter filter) where TD : class, new();
        IEnumerable<ModelSticker> Where(Expression<Func<ModelSticker, bool>> predicate);
        ModelSticker GetFirstOrDefault(Expression<Func<ModelSticker, bool>> predicate);
        int Add(ModelSticker model);
        int Update(ModelSticker model);
        int Delete(Expression<Func<ModelSticker, bool>> predicate);
    }
}