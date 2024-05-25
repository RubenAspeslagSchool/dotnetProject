using AutoMapper.QueryableExtensions;
using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.WebAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.Extensions;

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
        [FromQuery] string orderBy,
        [FromServices] IConfiguration config)
    {
        cardFilter.MaxPageSize = int.Parse(config["maxPageSize"]);

        IQueryable<Card> queryableCards = _cardRepository.GetAllCards().AsQueryable();

        queryableCards = queryableCards.Filter(
            cardFilter.CardName,
            cardFilter.CardText,
            cardFilter.ArtistName,
            cardFilter.SetCode,
            cardFilter.RarityCode);

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            queryableCards = ApplySorting(queryableCards, orderBy);
        }
        else
        {
            queryableCards = queryableCards.OrderBy(card => card.Id);
        }

        int totalRecords = await queryableCards.CountAsync();
        List<Card> pagedCards = await queryableCards
            .Skip((cardFilter.PageNumber - 1) * cardFilter.PageSize)
            .Take(cardFilter.PageSize)
            .ToListAsync();

        List<CardReadDTO> cardReadDtos = _mapper.Map<List<CardReadDTO>>(pagedCards);

        PagedResponse<IEnumerable<CardReadDTO>> result = new PagedResponse<IEnumerable<CardReadDTO>>(
            cardReadDtos,
            cardFilter.PageNumber,
            cardFilter.PageSize)
        {
            TotalRecords = totalRecords
        };

        return Ok(result);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<CardDetailDTO>> GetCardById(long id)
    {
        Card card = await _cardRepository.GetCardByIdAsync(id);
        if (card == null)
        {
            return NotFound();
        }

        CardDetailDTO cardDetailDto = _mapper.Map<CardDetailDTO>(card);
        return Ok(cardDetailDto);
    }

    private IQueryable<Card> ApplySorting(IQueryable<Card> query, string orderBy)
    {
        return orderBy.ToLower() switch
        {
            "name" => query.OrderBy(card => card.Name),
            "artist" => query.OrderBy(card => card.Artist.FullName),
            _ => query,
        };
    }
}
