using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.Filters;
using Microsoft.Extensions.Options;

public class RootQuery : ObjectGraphType
{
    public RootQuery(ICardRepository cardRepository,
        IArtistRepository artistRepository,
        IOptions<PaginationFilter> pagingFilter)
    {
        Name = "Query";
        PaginationFilter pagingOptions = pagingFilter.Value;

        #region Cards
        FieldAsync<ListGraphType<CardType>>(
            "Cards",
            Description = "Get all cards",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> { Name = "page", DefaultValue = pagingOptions.PageNumber },
                new QueryArgument<IntGraphType> { Name = "power" },
                new QueryArgument<IntGraphType> { Name = "toughness" }
            },
            resolve: async context =>
            {
                int page = context.GetArgument<int>("page");
                int? power = context.GetArgument<int?>("power");
                int? toughness = context.GetArgument<int?>("toughness");

                var cards = await cardRepository.GetCardsByPageAsync(page, pagingOptions.PageSize);

                if (power.HasValue)
                {
                    cards = cards.Where(c => c.Power == power.Value.ToString()).ToList();
                }

                if (toughness.HasValue)
                {
                    cards = cards.Where(c => c.Toughness == toughness.Value.ToString()).ToList();
                }

                return cards;
            }
        );
        #endregion

        #region Artists
        FieldAsync<ListGraphType<ArtistType>>(
            "Artists",
            Description = "Get all artists",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> { Name = "limit" }
            },
            resolve: async context =>
            {
                int? limit = context.GetArgument<int?>("limit");
                var artists = await artistRepository.GetAllArtistsAsync();

                if (limit.HasValue)
                {
                    return artists.Take(limit.Value).ToList();
                }

                return artists.ToList();
            }
        );

        FieldAsync<ArtistType>(
            "Artist",
            Description = "Get artist by ID",
            arguments: new QueryArguments
            {
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "artistId" }
            },
            resolve: async context =>
            {
                int artistId = context.GetArgument<int>("artistId");
                var artist = await artistRepository.GetArtistAsync(artistId);
                return artist;
            }
        );
        #endregion
    }
}
