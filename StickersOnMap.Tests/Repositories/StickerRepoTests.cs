namespace StickersOnMap.Tests.Repositories
{
    using System;
    using System.Linq;
    using AutoMapper;
    using DAL.DbContexts;
    using DAL.Interfaces;
    using DAL.Models;
    using DAL.Repositories;
    using NUnit.Framework;

    [TestFixture]
    public class StickerRepoTests : BaseRepositoryTest
    {
        private IStickerRepo _stickerRepo;
        private FakeStickersDb<ModelSticker> _stickerDb;
        
        [SetUp]
        public void SetUpStickerRepo()
        {
            var mapper = CreateSingletonMapper();
            _stickerDb = new FakeStickersDb<ModelSticker>();
            _stickerRepo = new StickerRepo(_stickerDb, mapper);
        }
        
        [Test]
        public void StickerRepoGetFirstOrDefaultTest()
        {
            // Arrange
            var name = new Guid().ToString();
            var stickers = CreateCollectionWithModifyElements<ModelSticker>(m =>
            {
                m.Active = false;
                m.Name = name;
            });
            
            var expected = stickers.Last();
            expected.Active = true;
            
            _stickerDb.SetCollection(stickers);

                // Act
            var result = _stickerRepo.GetFirstOrDefault(e=>e.Active);
            
            // Assert
            Assert<ModelSticker, ModelSticker>.AreEqual(expected, result);
            Assert.AreEqual(name, result.Name);
        }
        
        [Test]
        public void StickerRepoAddTest()
        {
            // Arrange
            var name = new Guid().ToString();
            var stickers = CreateCollectionWithModifyElements<ModelSticker>(m =>
            {
                m.Active = false;
            });

            var expected = new ModelSticker
            {
                Active = true,
                Name = name
            };

            _stickerDb.SetCollection(stickers);

            // Assert
            Assert.IsNull(_stickerRepo.GetFirstOrDefault(e=>e.Active));
            
            // Act
            _stickerRepo.Add(expected);
            var result = _stickerRepo.GetFirstOrDefault(e=>e.Active);
            
            // Assert
            Assert<ModelSticker, ModelSticker>.AreEqual(expected, result);
            Assert.AreEqual(name, result.Name);
        }
        
        [Test]
        public void StickerRepoUpdateTest()
        {
            // Arrange
            var name = new Guid().ToString();
            var stickers = CreateCollectionWithModifyElements<ModelSticker>(m =>
            {
                m.Active = false;
            });

            var expectedId = stickers.Last().Id;

            var expected = new ModelSticker
            {
                Id = expectedId,
                Active = true,
                Name = name
            };

            _stickerDb.SetCollection(stickers);

            // Assert
            Assert.IsNull(_stickerRepo.GetFirstOrDefault(e=>e.Active));
            
            // Act
            _stickerRepo.Update(expected);
            var result = _stickerRepo.GetFirstOrDefault(e=>e.Id == expectedId);
            
            // Assert
            Assert<ModelSticker, ModelSticker>.AreEqual(expected, result);
            Assert.AreEqual(name, result.Name);
        }
        
        [Test]
        public void StickerRepoDeleteTest()
        {
            // Arrange
            var name = new Guid().ToString();
            var stickers = CreateCollectionWithModifyElements<ModelSticker>(m =>
            {
                m.Active = false;
            });

            var expected = stickers.Last();
            expected.Active = true;
            expected.Name = name;
            var expectedId = stickers.Last().Id;

            _stickerDb.SetCollection(stickers);

            // Assert
            var expectedCurrent = _stickerRepo.GetFirstOrDefault(e => e.Id == expectedId);
            Assert.IsNotNull(expectedCurrent);
            Assert<ModelSticker, ModelSticker>.AreEqual(expected, expectedCurrent);
            
            // Act
            _stickerRepo.Delete(e=>e.Id == expectedId);
            var result = _stickerRepo.GetFirstOrDefault(e=>e.Id == expectedId);
            
            // Assert
            Assert.IsNull(result);
        }
    }
}