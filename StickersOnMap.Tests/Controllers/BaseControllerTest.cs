namespace StickersOnMap.Tests.Controllers
{
    using System;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoMapper;
    using NUnit.Framework;
    using WEB.Infrastructure.MappingProfiles;

    internal abstract class BaseControllerTest
    {
        private Fixture _fixture;
        private static readonly Lazy<MapperConfiguration> _mapperConfiguration =
            new Lazy<MapperConfiguration>(() => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfileSticker());
            }));

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }
        
        protected Fixture GetFixture() => _fixture;
        
        protected static IMapper CreateSingletonMapper()
        {
            var mapper = _mapperConfiguration.Value.CreateMapper();
            mapper.ConfigurationProvider.CompileMappings();
            return mapper;
        }
    }
}