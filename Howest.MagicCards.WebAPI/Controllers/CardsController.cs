﻿using AutoMapper;
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

namespace Howest.MagicCards.WebAPI.Controllers;

[Route("api/V{version:apiVersion}/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]

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
    [ApiVersion("1.1")]
    public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards(
        [FromQuery] CardFilter cardFilter,
        [FromServices] IConfiguration config
    )
    {
        cardFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
        IQueryable<Card> queryableCards = _cardRepository.GetAllCards().AsQueryable();
        queryableCards = queryableCards.Filter(cardFilter.CardName,cardFilter.CardText,cardFilter.ArtistName,cardFilter.SetCode,cardFilter.RarityCode,cardFilter.CardType);


        List<Card> pagedCards = await queryableCards.ToPagedListAsync(cardFilter.PageNumber, cardFilter.PageSize);
        IQueryable<CardReadDTO> cardReadDtos = pagedCards.AsQueryable().ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider);
        PagedResponse<IEnumerable<CardReadDTO>> result = new PagedResponse<IEnumerable<CardReadDTO>>(cardReadDtos, cardFilter.PageNumber, cardFilter.PageSize)
        {
            TotalRecords = await queryableCards.CountAsync()
        };

        return Ok(result);
    }

    [HttpGet]
    [ApiVersion("1.5")]
    public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards(
        [FromQuery] CardFilter cardFilter,
        [FromQuery] string order,
        [FromServices] IConfiguration config)
    {
        cardFilter.MaxPageSize = int.Parse(config["maxPageSize"]);

        IQueryable<Card> queryableCards = _cardRepository
            .GetAllCards()
            .AsQueryable()
            .AsNoTracking()
            .Filter(cardFilter.CardName,cardFilter.CardText,cardFilter.ArtistName,cardFilter.SetCode,cardFilter.RarityCode,cardFilter.CardType)
            .ApplySorting(order);
        Console.WriteLine(" cards=  " + queryableCards);

        int totalRecords = await queryableCards.CountAsync();
        
        List<Card> pagedCards = await queryableCards.ToPagedListAsync(cardFilter.PageNumber, cardFilter.PageSize) ;

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
    [ApiVersion("1.5")]

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
}
