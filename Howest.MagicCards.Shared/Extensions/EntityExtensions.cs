using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class EntityExtensions
    {
        public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> entities, int pageNumber, int pageSize)
        {
            return await entities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

       
    }
}
