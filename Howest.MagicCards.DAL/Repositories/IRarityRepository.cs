﻿using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IRarityRepository
    {
        Task<List<Rarity>> GetAllRaritiesAsync();
        Task<Rarity> GetRarityAsync(string code);
    }
}
