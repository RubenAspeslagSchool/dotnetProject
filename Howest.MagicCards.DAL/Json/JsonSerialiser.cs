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
    public class JsonSerialiser
    {
        private string jsonFilePath = "..\\Howest.MagicCards.DAL\\Json\\deck.json";
        public JsonSerialiser()
        {
        }

        public IList<Deck> GetDecks()
        {
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<IList<Deck>>(json);
            }
        }
      
        public void SaveDecks(List<Deck> decks)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(decks);
            File.WriteAllText(jsonFilePath, json);
        }
}

