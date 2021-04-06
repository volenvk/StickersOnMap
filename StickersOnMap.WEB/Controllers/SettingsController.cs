using Microsoft.AspNetCore.Mvc;

namespace StickersOnMap.WEB.Controllers
{
    using System;
    using Core.Infrastructure;
    using Infrastructure.Settings;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Models;

    /// <summary>
    /// Получение настроек приложения
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IAppSettings _settings;
        private readonly ILogger<SettingsController> _logger;

        /// <inheritdoc />
        public SettingsController(IAppSettings settings, ILogger<SettingsController> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получение настроек приложения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AppSettingsDTO> Get()
        {
            try
            {
                var uri = _settings.UriMap;

                if (!string.IsNullOrEmpty(uri))
                {
                    return Ok(new AppSettingsDTO
                    {
                        UriMap = uri
                    });   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка. Не удалось прочитать настройки приложения.");
            }
            
            return ErrorResult.NotFound("Ошибка: данные не найдены.");
        }
    }
}