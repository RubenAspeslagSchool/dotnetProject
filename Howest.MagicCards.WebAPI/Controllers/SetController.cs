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
    public class SetController : ControllerBase
    {
        private readonly ISetRepository _setRepository;
        private readonly IMapper _mapper;

        public SetController(ISetRepository setReposetory, IMapper mapper)
        {
            _setRepository = setReposetory;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetReadDTO>>> GetSets()
        {
            List<Set> allSets = await _setRepository.GetAllSetsAsync();
            if (allSets.Any())
            {
                List<SetReadDTO> setReadDtos = _mapper.Map<List<SetReadDTO>>(allSets);
                return Ok(setReadDtos);
            }
            else
            {
                return NotFound("No sets found");
            }
        }
    }
}
