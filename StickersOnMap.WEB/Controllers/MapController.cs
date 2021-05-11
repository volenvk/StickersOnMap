using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Newtonsoft.Json;

namespace StickersOnMap.WEB.Controllers
{
    using Core.Infrastructure;
    using DAL.Interfaces;
    using DAL.Models;
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
        private readonly IStickerRepoFacade _stickerRepoFacade;
        private readonly IMapper _mapper;


        /// <inheritdoc />
        public MapController(IStickerRepoFacade stickerRepoFacade, IMapper mapper, ILogger<MapController> logger)
        {
            _stickerRepoFacade = stickerRepoFacade ?? throw new ArgumentNullException(nameof(stickerRepoFacade));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                var activeGeoDatas = _stickerRepoFacade.Where(s=>s.Active)?.ToList();
                IEnumerable<GeoDataDTO> geoDataDtos = new GeoDataDTO[0];
                if (activeGeoDatas != null)
                {
                    geoDataDtos = _mapper.Map<IEnumerable<ModelSticker>, IEnumerable<GeoDataDTO>>(activeGeoDatas);
                }
                
                return Ok(geoDataDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось получить данные.");
            }
            
            return ErrorResult.NotFound("Ошибка: данные не найдены.");
        }
        
        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="dto">гео-данные</param>
        /// <returns></returns>
        [HttpPost("create")]
        [Produces(typeof(GeoDataDTO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<int> Create(GeoDataDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (dto != null)
                    {
                        if (_stickerRepoFacade.Any(m => m.Name.Equals(dto.Name, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            return ErrorResult.Conflict($"Ошибка: {dto.Name} было добавлено ранее.");
                        }
                    
                        var model = _mapper.Map<ModelSticker>(dto);

                        if (_stickerRepoFacade.Add(model) > 0)
                        {
                            _logger.LogInformation($"Создание {typeof(GeoDataDTO).Name}: [{ToJsonString(dto)}]");

                            return Ok(model.Id);
                        }
                        
                        return ErrorResult.BadRequest("Ошибка: данные не удалось добавить.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return ErrorResult.BadRequest("Ошибка: неверные данные.");
        }
        
        private string ToJsonString(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return string.Empty;
        }
    }
}