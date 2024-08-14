using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mapping
{
    public class CardsProfile : Profile
    {
        public CardsProfile()
        {
            // Mapping for Card and DTOs
            CreateMap<Card, CardReadDTO>()
                .ForMember(dto => dto.ImageUrl, opt => opt.MapFrom(c => c.OriginalImageUrl))
                .ForMember(dto => dto.Artist, opt => opt.MapFrom(c => c.Artist.FullName));
            CreateMap<Card, CardDetailDTO>();

            // Mapping for Artist and ArtistReadDTO
            CreateMap<Artist, ArtistReadDTO>()
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(a => a.FullName));

            // Mapping for Rarity and RarityReadDTO
            CreateMap<Rarity, RarirtyReadDTO>()
                .ForMember(dto => dto.RarityName, opt => opt.MapFrom(r => r.Name));

            // Mapping for Set and SetReadDTO
            CreateMap<Set, SetReadDTO>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(dto => dto.Code, opt => opt.MapFrom(s => s.Code));
        }
    }
}
