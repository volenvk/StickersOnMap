namespace StickersOnMap.DAL.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public interface IStickersDb : IDbContext
    {
        DbSet<ModelSticker> Stickers { get; }
    }
}