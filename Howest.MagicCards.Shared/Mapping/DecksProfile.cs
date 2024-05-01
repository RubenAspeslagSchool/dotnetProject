using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mapping
{
    public class DecksProfile : Profile 
    {
        public DecksProfile() 
        {
            CreateMap<DeckCreateDTO, Deck>()
                           .ForMember(deck => deck.CardDecks,
                                       opt => 
                                       opt.MapFrom(dto => new List<CardDeck>())
                           );

            CreateMap<Deck, DeckReadDTO>()
                .ForMember(dto => dto.CardDecks,
                    opt => opt.MapFrom(deck => deck.CardDecks.Select(cd => new CardDeckReadDTO { CardId = (long)cd.CardId, Amount = cd.Amount }))
                );

            CreateMap<CardDeck, CardDeckReadDTO>();
        }
    }
}
