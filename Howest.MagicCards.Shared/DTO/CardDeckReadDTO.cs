using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public record CardDeckReadDTO
    {
        public long CardId { get; init; }
        public int DeckId { get; init; }
        public int Amount { get; init; }
    }
}
