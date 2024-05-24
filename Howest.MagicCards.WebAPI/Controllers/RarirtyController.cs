using Howest.MagicCards.DAL.Models;
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
        public async Task<ActionResult<IEnumerable<RarirtyReadDTO>>> Getrarities()
        {
            List<Rarity> allRarities = await _rarityRepository.GetAllRaritiesAsync();
            if (allRarities.Any())
            {
                List<RarirtyReadDTO> rarityReadDtos = allRarities.Select(r => new RarirtyReadDTO { RarityName = r.Name }).ToList();
                return Ok(rarityReadDtos);
            }
            else
            {
                return NotFound("No rarities found");
            }
        }
    }
}
