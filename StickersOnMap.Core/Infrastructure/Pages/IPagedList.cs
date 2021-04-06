using System.Collections.Generic;

namespace StickersOnMap.Core.Infrastructure.Pages
{
    public interface IPagedList<out T>
    {
        IEnumerable<T> Data { get; }
        /// <summary>
        /// Количество элементов в списке после применения фильтра
        /// В общем случае TotalCount != Data.Count()
        /// </summary>
        int TotalCount { get; }
    }
}