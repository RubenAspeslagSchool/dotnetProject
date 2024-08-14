using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/V{version:apiVersion}/[controller]")]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any)]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypesReposetory _typesReposetory;
        private readonly IMapper _mapper;

        public TypeController(ITypesReposetory typesReposetory, IMapper mapper)
        {
            _typesReposetory = typesReposetory;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeReadDTO>>> GetTypes()
        {
            List<DAL.Models.Type> allRarities = await _typesReposetory.GetAllTypesAsync();
            if (allRarities.Any())
            {
                List<TypeReadDTO> rarityReadDtos = allRarities.Select(t => new TypeReadDTO { Id = t.Id, Name = t.Name }).ToList();
                return Ok(rarityReadDtos);
            }
            else
            {
                return NotFound("No types found");
            }
        }
    }
}
