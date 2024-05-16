using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.ViewModels
{
    public class DeckViewModel
    {
        public string? DeckName { get; set; }
        public IEnumerable<DeckCardViewModel>? DeckCards { get; set; }
    }
}
