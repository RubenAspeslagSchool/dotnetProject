using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlTypesRepository : ITypesRepository
    {
        private readonly MtgV1Context _db;

        public SqlTypesRepository(MtgV1Context mtgContext)
        {
            _db = mtgContext;
        }

        public async Task<List<Models.Type>> GetAllTypesAsync()
        {
            return await _db.Types.ToListAsync();
        }
    }
}
