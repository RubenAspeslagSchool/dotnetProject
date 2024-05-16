using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.ViewModels
{
    public class DeckCardViewModel
    {
        public int? DeckId { get; set; }
        public long? CardId { get; set; }
        public string? CardName { get; set; }
        public int? Amount { get; set; }
    }
}
