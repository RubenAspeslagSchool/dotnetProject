using Howest.MagicCards.DAL.Models;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.DAL.Json;

    public class JsonRepository
    {
        private string jsonFilePath = "..\\Howest.MagicCards.DAL\\Json\\deck.json";

        public JsonRepository()
        {
       
        }

        public IList<Deck> LoadJson()
        {
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<IList<Deck>>(json);
            }
        }

        public IList<Deck> getDecks()
        {
            return LoadJson();
        }

        public void SaveDecks(List<Deck> decks)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(decks);
            File.WriteAllText(jsonFilePath, json);
        }
    
    public void SaveDeck(long deckId, Deck newDeck)
    {
            List<Deck> decks = getDecks().ToList();   
            Deck oldDeck = decks.FindLast(deck => deck.Id == deckId);
             if (oldDeck is Deck && oldDeck is not null) 
             {
                oldDeck.DeckName = newDeck.DeckName;
                oldDeck.CardDecks = newDeck.CardDecks;
             }
            else
            {
                decks.Add(newDeck);
            }   
            SaveDecks(decks);
     }
}

