using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public record DeckReadDTO
    {
        public long Id { get; init; }
        public string DeckName { get; init; }
        public virtual List<CardDeckReadDTO> CardDecks { get; init; }
    }
}
