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
        private readonly ISetReposetory _setReposetory;

       public SetController(ISetReposetory setReposetory)
        {
            _setReposetory = setReposetory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetReadDTO>>> GetSets()
        {
            List<Set> allSets = await _setReposetory.GetAllSetsAsync();
            if (allSets.Any())
            {
                List<SetReadDTO> setReadDtos = allSets.Select(r => new SetReadDTO { Name = r.Name, Code = r.Code }).ToList();
                return Ok(setReadDtos);
            }
            else
            {
                return NotFound("No sets found");
            }
        }
    }
}
