using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlSetReposetory : ISetReposetory
    {
        private readonly MtgV1Context _db;

        public SqlSetReposetory(MtgV1Context mtgContext)
        {
            _db = mtgContext;
        }
        public async Task<List<Set>> GetAllSetsAsync()
        {
            return await _db.Sets.ToListAsync();
        }

    }
}
