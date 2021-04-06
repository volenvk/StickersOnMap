using System;
using System.Linq.Expressions;

namespace StickersOnMap.DAL.Interfaces
{
    using System.Linq;
    using Models;
    
    public interface IStickerRepo
    {
        IQueryable<ModelSticker> All();
        bool Any(Expression<Func<ModelSticker, bool>> predicate = null);
        IQueryable<ModelSticker> Where(Expression<Func<ModelSticker, bool>> predicate);
        ModelSticker Get(Expression<Func<ModelSticker, bool>> predicate);
        int Add(ModelSticker model);
        int Update(ModelSticker model);
        int Delete(Expression<Func<ModelSticker, bool>> predicate);
    }
}