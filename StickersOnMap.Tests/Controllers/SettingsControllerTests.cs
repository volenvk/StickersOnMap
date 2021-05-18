namespace StickersOnMap.Tests.Controllers
{
    using AutoFixture;
    using Core.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;
    using WEB.Controllers;
    using WEB.Infrastructure.Settings;
    using WEB.Models;

    [TestFixture]
    public class SettingsControllerTests : BaseControllerTest
    {
        private class TestableController : SettingsController
        { 
            public static TestableController Create(IAppSettings config)
            {
                var fakeLogger = Substitute.For<ILogger<SettingsController>>();
                return new TestableController(config, fakeLogger);
            }
            
            private TestableController(IAppSettings settings, ILogger<SettingsController> logger) : base(settings, logger) { }
        }

        /// <summary>
        /// Проверяем что uriMap is ok
        /// Дано:
        ///     контроллер SettingsController
        /// Если:
        ///     вызвать метод Get
        /// То:
        ///     Получаем AppSettingsDTO
        /// </summary>
        [Test]
        public void SettingsControllerPositiveTest()
        {
            // Arrange
            var uriMap = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png";
            var statusCode = 200;
            var fakeConfig = GetFixture().Build<AppSettings>()
                .With(o => o.UriMap, uriMap)
                .Create();

            var controller = TestableController.Create(fakeConfig);

            // Act
            var actionResult = controller.Get();
            
            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(statusCode, result.StatusCode);
            var resultDTO = result.Value as AppSettingsDTO;
            Assert.IsNotNull(resultDTO);
            Assert.AreEqual(uriMap, resultDTO.UriMap);
        }
        
        /// <summary>
        /// Проверяем что uriMap is string.Empty
        /// Дано:
        ///     контроллер SettingsController
        /// Если:
        ///     вызвать метод Get
        /// То:
        ///     Получаем ErrorResult
        /// </summary>
        [Test]
        public void SettingsControllerNegativeTest()
        {
            // Arrange
            var uriMap = string.Empty;
            var statusCode = 404;
            var fakeConfig = GetFixture().Build<AppSettings>()
                .With(o => o.UriMap, uriMap)
                .Create();

            var controller = TestableController.Create(fakeConfig);

            // Act
            var actionResult = controller.Get();
            
            // Assert
            var result = actionResult.Result as ErrorResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(statusCode, result.StatusCode);
        }
    }
}