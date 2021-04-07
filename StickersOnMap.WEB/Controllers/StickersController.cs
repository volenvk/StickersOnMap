using Microsoft.AspNetCore.Mvc;

namespace StickersOnMap.WEB.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Core.Infrastructure;
    using Core.Infrastructure.Extensions;
    using Core.Infrastructure.Filteres;
    using Core.Infrastructure.Pages;
    using DAL.Interfaces;
    using DAL.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Models;
    using Newtonsoft.Json;

    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StickersController : ControllerBase
    {
        private readonly ILogger<StickersController> _logger;
        private readonly IStickerRepo _stickerRepo;
        private readonly IMapper _mapper;
        
        public StickersController(IStickerRepo stickerRepo, IMapper mapper, ILogger<StickersController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stickerRepo = stickerRepo ?? throw new ArgumentNullException(nameof(stickerRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="request">фильтр</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PagedList<StickerDTO>> FilterBy([FromBody] TableFilter request)
        {
            try
            {
                var query = _stickerRepo.All().FilterBy(request.Filters);
                
                var ids = query
                    .OrderByEntity(string.IsNullOrEmpty(request.Sort?.Property) ? null : request.Sort)
                    .TakePage(request.Page?.PageSize == 0 ? null : request.Page)
                    .Select(r => r.Id)
                    .ToHashSet();

                var pagedList = _stickerRepo.Where(r => ids.Contains(r.Id))
                    .OrderByEntity(string.IsNullOrEmpty(request.Sort?.Property) ? null : request.Sort)
                    .ProjectTo<StickerDTO>(_mapper.ConfigurationProvider)
                    .AsPagedList(ids.Count);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return ErrorResult.NotFound("Ошибка: данные не найдены.");
        }

        /// <summary>
        /// Получить запись по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StickerDTO> Get(int id)
        {
            try
            {
                var model = _stickerRepo.Get(t=>t.Id == id);

                if (model == null)
                    return ErrorResult.NotFound("Ошибка: данные не найдены.");

                return Ok(model);   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return ErrorResult.NotFound("Ошибка: данные не найдены.");
        }

        /// <summary>
        /// Обновление записи
        /// </summary>
        /// <param name="dto">стикер</param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(StickerDTO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Update(StickerDTO dto)
        {
            try
            {
                if (ModelState.IsValid && !string.IsNullOrEmpty(dto.Name))
                {
                    var model = _stickerRepo.Get(s => s.Id == dto.Id);

                    if (model != null)
                    {
                        model.Active = dto.Active ?? false;
                        model.Name = dto.Name;

                        if (_stickerRepo.Update(model) > 0)
                        {
                            _logger.LogInformation($"Обновление данных {typeof(StickerDTO).Name}: [{ToJsonString(dto)}]");

                            return Ok(model.Id);
                        }
                    
                        return ErrorResult.BadRequest("Ошибка: данные не удалось обновить.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return ErrorResult.BadRequest("Ошибка: неверные данные.");
        }
        
        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                var deletedRow = _stickerRepo.Delete(item=> item.Id == id);
                if (deletedRow > 0)
                {
                    _logger.LogInformation($"Удаление {typeof(StickerDTO).Name} с id: [{id}]");
            
                    return Ok(deletedRow);
                }

                return ErrorResult.NotFound("Ошибка: данные не удалось удалить.");
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