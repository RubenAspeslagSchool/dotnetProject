using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Exceptions
{
    public class ToManyCardsInDeckExeption : Exception
    {
         public ToManyCardsInDeckExeption() : base("There can only be 60 cards in 1 deck!") { }
    }
}
