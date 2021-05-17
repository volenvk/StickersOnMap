using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StickersOnMap.WEB.Controllers
{
    using Core.Infrastructure;
    using Core.Infrastructure.Filteres;
    using Core.Infrastructure.Pages;
    using DAL.Interfaces;
    using Models;


    /// <summary>
    /// Контроллер работы с таблицей стикеров
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StickersController : ControllerBase
    {
        private readonly IStickerRepo _stickerRepoFacade;
        private readonly ILogger<StickersController> _logger;

        /// <inheritdoc />
        public StickersController(IStickerRepo stickerRepoFacade, ILogger<StickersController> logger)
        {
            _stickerRepoFacade = stickerRepoFacade ?? throw new ArgumentNullException(nameof(stickerRepoFacade));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                if (request != null)
                {
                    return _stickerRepoFacade.GetPages<StickerDTO>(request);
                }
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
                var model = _stickerRepoFacade.GetFirstOrDefault(t=>t.Id == id);

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
                    var model = _stickerRepoFacade.GetFirstOrDefault(s => s.Id == dto.Id);
                    if (model != null)
                    {
                        model.Active = dto.Active ?? model.Active;
                        model.Name = dto.Name;

                        if (_stickerRepoFacade.Update(model) > 0)
                        {
                            _logger.LogInformation($"Обновление данных {typeof(StickerDTO).Name}: [{ToJsonString(dto)}]");

                            return Ok(dto.Id);
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
                var deletedRow = _stickerRepoFacade.Delete(item=> item.Id == id);
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