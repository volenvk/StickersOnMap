using System.Collections.Generic;

namespace StickersOnMap.Core.Infrastructure.Pages
{
    public class PagedList<T> : IPagedList<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
    }
}