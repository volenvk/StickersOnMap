using Microsoft.EntityFrameworkCore;

namespace StickersOnMap.DAL.DbContexts
{
    using Models;
    
    public class StickersDb : DbContext, IStickersDb
    {
        public StickersDb(DbContextOptions<StickersDb> options) : base(options) { }
        
        public DbSet<ModelSticker> Stickers { get; }
    }
}