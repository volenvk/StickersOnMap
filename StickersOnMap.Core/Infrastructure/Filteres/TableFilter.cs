using System.Collections.Generic;

namespace StickersOnMap.Core.Infrastructure.Filteres
{
    using Pages;
    
    public class TableFilter
    {
        public IEnumerable<Filter> Filters { get; set; }
        public Page Page { get; set; }
        public Sort Sort { get; set; }

        public override string ToString()
        {
            return $"Sort: {Sort}, Page: {Page}, Filters: {(Filters == null ? string.Empty : string.Join(",", Filters))}";
        }
    }
}