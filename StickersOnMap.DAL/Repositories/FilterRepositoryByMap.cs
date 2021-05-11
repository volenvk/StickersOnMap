using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using NLog;

namespace StickersOnMap.DAL.Repositories
{
    using Core.Infrastructure.Filteres;
    using Core.Infrastructure.Pages;
    using Core.Interfaces;
    using Extensions;
    using Interfaces;

    public class FilterRepositoryByMap<T> : IFilterRepositoryByMap<T> where T : class, IEntity
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FilterRepositoryByMap(IRepositoryConfig config, IMapper mapper)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = LogManager.GetCurrentClassLogger();
            _uow = config.CreateSingletonUnitOfWork();
        }
        
        public IEnumerable<TD> FilterBy<TD>(TableFilter filter) where TD : class, new()
        {
            try
            {
                return _uow.GetQueryable<T>().FilterBy(filter.Filters)
                    .OrderByEntity(string.IsNullOrEmpty(filter.Sort?.Property) ? null : filter.Sort)
                    .ProjectTo<TD>(_mapper.ConfigurationProvider)
                    .ToList();
            }
            catch (Exception ex)
            {
                if (filter != null)
                    ex.Data.Add("params:", filter.ToString());
                _logger.Error(ex, $"[{nameof(FilterBy)}] " + ex.Message);
                return null;
            }
        }

        public PagedList<TD> GetPages<TD>(TableFilter filter) where TD : class, new()
        {
            try
            {
                var count = _uow.GetQueryable<T>().FilterBy(filter.Filters).Count();
                
                return _uow.GetQueryable<T>().FilterBy(filter.Filters)
                    .OrderByEntity(string.IsNullOrEmpty(filter.Sort?.Property) ? null : filter.Sort)
                    .TakePage(filter.Page?.PageSize == 0 ? null : filter.Page)
                    .ProjectTo<TD>(_mapper.ConfigurationProvider)
                    .AsPagedList(count);
            }
            catch (Exception ex)
            {
                if (filter != null)
                    ex.Data.Add("params:", filter.ToString());
                _logger.Error(ex, $"[{nameof(GetPages)}] " + ex.Message);
                return null;
            }
        }
    }
}