namespace StickersOnMap.Core.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Reflection;
    using Filteres;
    using Pages;
    using Interfaces;
    
    public static class QueryableExtension
    {
        
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> source, IEnumerable<Filter> filters)
        {
            return filters.Aggregate(source, (current, filter) => current.FilterBy(filter.Property, filter.Value));
        }
        
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> source, string propertyName, string value) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException($"property null or empty on object {typeof(T).Name}");
            
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase);

            if (propertyInfo == null)
                throw new ArgumentException($"property {propertyName} not found on object {typeof(T).Name}");

            return source.Where($"{propertyName}.Contains(@0)", value);
        }
        
        public static IQueryable<T> TakePage<T>(this IQueryable<T> src, Page page)
        {
            return page == null ? src : src.TakePage(page.PageNumber, page.PageSize);
        }
        
        public static PagedList<T> AsPagedList<T>(this IQueryable<T> src, int totalCount)
        {
            return new PagedList<T>
            {
                Data = src.ToList(),
                TotalCount = totalCount
            };
        }
        
        public static IQueryable<T> OrderByEntity<T>(this IQueryable<T> src, Sort sort) where T : IEntity
        {
            return sort == null ? src : OrderByProp(src, sort.Property, sort.Reverse).ThenBy(e => e.Id);
        }
        
        private static IOrderedQueryable<T> OrderByProp<T>(IQueryable<T> src, string propertyName, bool reverse = false)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException($"property null or empty on object {typeof(T).Name}");
            
            var propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo == null)
                throw new ArgumentException($"property {propertyName} not found on object {typeof(T).Name}");

            var ordering = reverse ? "ASC" : "DESC";
            
            return src.OrderBy(ordering, propertyName);
        }
        
        private static IQueryable<T> TakePage<T>(this IQueryable<T> src, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return src?.Take(10);

            return src?.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}