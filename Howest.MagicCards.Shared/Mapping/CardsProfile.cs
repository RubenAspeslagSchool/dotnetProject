using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mapping
{
    public class CardsProfile: Profile
    {
        public CardsProfile()
        {
            CreateMap<Card, CardReadDTO>()
                .ForMember(dto => dto.ImageUrl,
                            opt => opt.MapFrom(c => c.OriginalImageUrl))
                .ForMember(dto => dto.Rarity,
                            opt => opt.MapFrom(c => c.Rarity.Name))
                .ForMember(dto => dto.Artist,
                            opt => opt.MapFrom(c => c.Artist.FullName))
                .ForMember(dto => dto.Set,
                            opt => opt.MapFrom(c => c.Set.Name));
        }
    }
}
