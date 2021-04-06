namespace StickersOnMap.WEB.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using Core.Infrastructure;
    using DAL.Interfaces;
    using DAL.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Models;

    /// <summary>
    /// Контроллер работы с картой
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MapController : ControllerBase
    {
        private readonly ILogger<MapController> _logger;
        private readonly IGeoDataRepo _geoDataRepo;
        private readonly IMapper _mapper;

        
        public MapController(IGeoDataRepo geoDataRepo, IMapper mapper, ILogger<MapController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _geoDataRepo = geoDataRepo ?? throw new ArgumentNullException(nameof(geoDataRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        /// <summary>
        /// Получить данные для карты
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<GeoDataDTO>> GetAll()
        {
            try
            {
                var activeGeoDatas = _geoDataRepo.GetActive();
                if (activeGeoDatas != null)
                {
                    var geoDataDtos = _mapper.Map<IEnumerable<ModelGeoData>, IEnumerable<GeoDataDTO>>(activeGeoDatas);
                    return Ok(geoDataDtos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось получить данные.");
            }
            
            return ErrorResult.NotFound("Ошибка: данные не найдены.");
        }
    }
}