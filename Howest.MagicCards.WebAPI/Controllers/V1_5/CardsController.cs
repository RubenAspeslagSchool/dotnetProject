using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.WebAPI.Wrappers;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Extensions;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.WebAPI.Controllers.V1_5
{
    [ApiVersion("1.5")]
    [Route("api/V{version:apiVersion}/[controller]")]
    [ApiController]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any)]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards(
            [FromQuery] CardFilter cardFilter,
            [FromServices] IConfiguration config
        )
        {
            cardFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
            if (await _cardRepository.GetAllCardsAsync() is IQueryable<Card> allCards)
            {
                allCards = allCards.Filter(
                    cardFilter.CardName,
                    cardFilter.CardText,
                    cardFilter.ArtistName,
                    cardFilter.SetCode,
                    cardFilter.RarityCode);

                // Apply default ordering before pagination
                allCards = allCards.OrderBy(card => card.Id);

                var pagedCards = await allCards.ToPagedListAsync(cardFilter.PageNumber, cardFilter.PageSize);

                var cardReadDtos = pagedCards.AsQueryable().ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider);

                var result = new PagedResponse<IEnumerable<CardReadDTO>>(await cardReadDtos.ToListAsync(), cardFilter.PageNumber, cardFilter.PageSize)
                {
                    TotalRecords = await allCards.CountAsync()
                };

                return Ok(result);
            }
            else
            {
                return NotFound("No cards found");
            }
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<CardDetailDTO>> GetCardById(long id)
        {
            var card = await _cardRepository.GetCardByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            var cardDetailDto = _mapper.Map<CardDetailDTO>(card);
            return Ok(cardDetailDto);
        }
    }
}
