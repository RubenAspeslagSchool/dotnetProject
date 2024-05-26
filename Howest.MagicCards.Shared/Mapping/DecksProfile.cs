using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;

namespace Howest.MagicCards.Shared.Mapping
{
    public class DecksProfile : Profile
    {
        public DecksProfile()
        {
            CreateMap<DeckViewModel, DeckCreateDTO>()
                .ForMember(dest => dest.DeckName, opt => opt.MapFrom(src => src.DeckName));

            CreateMap<DeckCreateDTO, Deck>()
                .ForMember(deck => deck.CardDecks, opt => opt.MapFrom(dto => new List<CardDeck>()));

            // Mapping from Deck to DeckReadDTO
            CreateMap<Deck, DeckReadDTO>()
                .ForMember(dto => dto.CardDecks, opt => opt.MapFrom(deck => deck.CardDecks.Select(cd => new CardDeckReadDTO { CardId = (long)cd.CardId, Amount = cd.Amount })));

            // Mapping from DeckReadDTO to Deck
            CreateMap<DeckReadDTO, Deck>()
                .ForMember(deck => deck.CardDecks, opt => opt.MapFrom(dto => dto.CardDecks.Select(cdr => new CardDeck { CardId = cdr.CardId, Amount = cdr.Amount })));

            CreateMap<CardDeck, CardDeckReadDTO>();

            // Ensure mapping for CardDeckReadDTO to CardDeck
            CreateMap<CardDeckReadDTO, CardDeck>()
                .ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.CardId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
        }
    }
}
