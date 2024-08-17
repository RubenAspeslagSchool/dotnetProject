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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;


        public ArtistController(IArtistRepository artistReposetory, IMapper mapper)
        {
            _artistRepository = artistReposetory;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistReadDTO>>> GetArtists()
        {
            List<Artist> allArtists = await _artistRepository.GetAllArtistsAsync();
            if (allArtists.Any())
            {
                List<ArtistReadDTO> setReadDtos = _mapper.Map<List<ArtistReadDTO>>(allArtists);
                return Ok(setReadDtos);
            }
            else
            {
                return NotFound(new List<ArtistReadDTO>());
            }
        }
    }
}
