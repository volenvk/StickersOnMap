using System.Collections.Generic;

namespace StickersOnMap.Core.Infrastructure.Filteres
{
    using Pages;
    
    public class TableFilter
    {
        public IEnumerable<Filter> Filters { get; set; }
        public Page Page { get; set; }
        public Sort Sort { get; set; }
    }
}