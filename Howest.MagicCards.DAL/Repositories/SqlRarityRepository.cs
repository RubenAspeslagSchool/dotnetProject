﻿using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlRarityRepository : IRarityRepository
    {
        private readonly MtgContext _db;
        public SqlRarityRepository(MtgContext mtgContext)
        {
            _db = mtgContext;
        }
        public IQueryable<Rarity> GetAllRarities()
        {
            return _db.Rarities.Select(a => a);
        }
    }
}
