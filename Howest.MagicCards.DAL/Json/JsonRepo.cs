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

    public class JsonRepo
    {
        private string jsonFilePath = "..\\Howest.MagicCards.DAL\\Json\\deck.json";

        public JsonRepo()
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

        public void save(List<Deck> decks)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(decks);
            File.WriteAllText(jsonFilePath, json);
        }

    public void save(long id, Deck deck)
    {
            List<Deck> decks = getDecks().ToList();
            if (decks.Any(x => x.Id == id))
            {
                foreach (Deck deck1 in decks)
                {
                    if (deck1.Id == id)
                    {
                        deck1.DeckName = deck.DeckName; 
                        deck1.CardDecks = deck.CardDecks;
                    }
                   
                }
            }
            else
            {
                decks.Add(deck);
            }   
            save(decks);
        }
    }

