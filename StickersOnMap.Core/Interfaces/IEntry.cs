using System;

namespace StickersOnMap.Core.Interfaces
{
    public interface IEntry : IEntity
    {
        bool Active { get; set; }
        DateTime CreateDate { get; set; }
    }
}