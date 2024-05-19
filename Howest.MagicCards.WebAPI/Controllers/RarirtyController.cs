﻿using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/V{version:apiVersion}/[controller]")]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any)]
    [ApiController]
    public class RarirtyController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepository;

        public RarirtyController(IRarityRepository rarityRepository)
        {
            _rarityRepository = rarityRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RarirtyReadDTO>> GetArtists()
        {
            return (_rarityRepository.GetAllRarities() is IQueryable<Rarity> allRarities) ?
                 Ok(allRarities.Select(r => new RarirtyReadDTO() { RarityName = r.Name}))
                 : NotFound("No artists found");
        }
    }
}
