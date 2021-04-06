namespace StickersOnMap.WEB.Infrastructure.MappingProfiles
{
    using AutoMapper;
    using DAL.Models;
    using Models;

    public class MappingProfileSticker : Profile
    {
        public MappingProfileSticker()
        {
            CreateMap<ModelSticker, StickerDTO>();
            CreateMap<StickerDTO, ModelSticker>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreateDate, o => o.Ignore());
            
            CreateMap<ModelGeoData, GeoDataDTO>();
            CreateMap<GeoDataDTO, ModelGeoData>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreateDate, o => o.Ignore());
        }
    }
}