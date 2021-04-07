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
            
            CreateMap<ModelSticker, GeoDataDTO>();
            CreateMap<GeoDataDTO, ModelSticker>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Active, o => o.MapFrom(t=> true))
                .ForMember(d => d.CreateDate, o => o.Ignore());
        }
    }
}