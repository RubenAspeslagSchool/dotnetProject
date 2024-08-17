using AutoMapper;
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
    public class RarityController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepository;
        private readonly IMapper _mapper;
        public RarityController(IRarityRepository rarityRepository, IMapper mapper)
        {
            _rarityRepository = rarityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RarityReadDTO>>> GetRarities()
        {
            List<Rarity> allRarities = await _rarityRepository.GetAllRaritiesAsync();
            if (allRarities.Any())
            {                
                List<RarityReadDTO> rarityReadDtos = _mapper.Map<List<RarityReadDTO>>(allRarities);
                return Ok(rarityReadDtos);
            }
            else
            {
                return NotFound("No rarities found");
            }
        }
    }
}
