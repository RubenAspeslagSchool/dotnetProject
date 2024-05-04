using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.WebAPI.Wrappers;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Mapping;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;

namespace Howest.MagicCards.WebAPI.Controllers;

[ApiVersion("1.1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
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
    public ActionResult<PagedResponse<IEnumerable<CardReadDTO>>> GetCards(
        [FromQuery] CardFilter cardFilter,
        [FromServices] IConfiguration config
    )
    {
        cardFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
        if (_cardRepository.GetAllCards() is IQueryable<Card> allCards)
        {
            allCards = allCards.Filter(cardFilter);

            PagedResponse<IEnumerable<CardReadDTO>> result = new PagedResponse<IEnumerable<CardReadDTO>>(
                 allCards.ToPagedList(cardFilter.PageNumber, cardFilter.PageSize)
                     .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                     .ToList(),
                 cardFilter.PageNumber,
                 cardFilter.PageSize)
            {
                TotalRecords = allCards.Count()
            };

            return Ok(result);
        }
        else
        {
            return NotFound("No cards found");
        }
    }

}
