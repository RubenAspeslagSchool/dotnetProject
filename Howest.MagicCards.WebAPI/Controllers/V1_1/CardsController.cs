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

namespace Howest.MagicCards.WebAPI.Controllers.V1_1;

[ApiVersion("1.1")]
[Route("api/V{version:apiVersion}/[controller]")]
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
    public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards(
        [FromQuery] CardFilter cardFilter,
        [FromServices] IConfiguration config
    )
    {
        cardFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
        IQueryable<Card> queryableCards = _cardRepository.GetAllCards().AsQueryable();

        queryableCards = queryableCards.Filter(
                cardFilter.CardName,
                cardFilter.CardText,
                cardFilter.ArtistName,
                cardFilter.SetCode,
                cardFilter.RarityCode);


            List<Card> pagedCards = await queryableCards.ToPagedListAsync(cardFilter.PageNumber, cardFilter.PageSize);
            IQueryable<CardReadDTO> cardReadDtos = pagedCards.AsQueryable().ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider);
            PagedResponse<IEnumerable<CardReadDTO>> result = new PagedResponse<IEnumerable<CardReadDTO>>(cardReadDtos, cardFilter.PageNumber, cardFilter.PageSize)
            {
                TotalRecords = await queryableCards.CountAsync()
            };

            return Ok(result);
      
    }
}
