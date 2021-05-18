namespace StickersOnMap.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoFixture;
    using AutoMapper;
    using DAL.Interfaces;
    using DAL.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NSubstitute.ReturnsExtensions;
    using NUnit.Framework;
    using WEB.Controllers;
    using WEB.Models;

    public class MapControllerTests : BaseControllerTest
    {
        private class TestableController : MapController
        {
            public static TestableController Create(IStickerRepo stickerRepo)
            {
                var fakeMapper = CreateSingletonMapper();
                var fakeLogger = Substitute.For<ILogger<MapController>>();
                return new TestableController(stickerRepo, fakeMapper, fakeLogger);
            }
            
            private TestableController(IStickerRepo stickerRepo, IMapper mapper, ILogger<MapController> logger)
                : base(stickerRepo, mapper, logger) { }
        }
        
        /// <summary>
        /// Проверяем что StickerRepo.Where() = collection with a element
        /// Дано:
        ///     контроллер MapController
        /// Если:
        ///     вызвать метод GetAll
        /// То:
        ///     Получаем
        /// </summary>
        [Test]
        public void MapControllerGetAllPositiveTest()
        {
            // Arrange
            var statusCode = 200;
            var count = 1;
            var stickers = GetFixture().Build<ModelSticker>()
                .With(s=>s.Active, true)
                .CreateMany(count).ToList();
            var fakeRepo = Substitute.For<IStickerRepo>();
            fakeRepo.Where(Arg.Any<Expression<Func<ModelSticker, bool>>>()).Returns(stickers);
            
            var controller = TestableController.Create(fakeRepo);

            // Act
            var actionResult = controller.GetAll();
            
            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(statusCode, result.StatusCode);
            var resultDTO = result.Value as IEnumerable<GeoDataDTO>;
            Assert.IsNotNull(resultDTO);
            Assert.IsTrue(resultDTO.Any());
        }
        
        /// <summary>
        /// Проверяем что StickerRepo.Where() = null
        /// Дано:
        ///     контроллер MapController
        /// Если:
        ///     вызвать метод GetAll
        /// То:
        ///     Получаем EmptyCollection
        /// </summary>
        [Test]
        public void MapControllerGetAllPositiveEmptyCollectionTest()
        {
            // Arrange
            var statusCode = 200;
            var count = 1;
            var fakeRepo = Substitute.For<IStickerRepo>();
            fakeRepo.Where(Arg.Any<Expression<Func<ModelSticker, bool>>>()).ReturnsNull();
            
            var controller = TestableController.Create(fakeRepo);

            // Act
            var actionResult = controller.GetAll();
            
            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(statusCode, result.StatusCode);
            var resultDTO = result.Value as IEnumerable<GeoDataDTO>;
            Assert.IsNotNull(resultDTO);
            Assert.IsFalse(resultDTO.Any());
        }
    }
}