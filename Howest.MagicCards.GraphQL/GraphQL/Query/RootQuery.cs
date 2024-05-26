using GraphQL;
using GraphQL.Types;
using GraphQLAPI.GraphQLTypes;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.Filters;
using Microsoft.EntityFrameworkCore;
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
            },
            resolve: async context =>
            {
                int page = context.GetArgument<int>("page");
                // Assuming ToPagedListAsync is an extension method you need to implement or replace with appropriate logic
                var cards = await cardRepository.GetAllCards()
                    .Skip((page - 1) * pagingOptions.PageSize)
                    .Take(pagingOptions.PageSize)
                    .ToListAsync(); // Use EF Core's ToListAsync for async operation
                return cards;
            }
        );
        #endregion

        #region Artists
        FieldAsync<ListGraphType<ArtistType>>(
            "Artists",
            Description = "Get All Artists",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> {Name = "limit", DefaultValue = pagingOptions.PageSize}
            },
            resolve: async context =>
            {
                int limit = context.GetArgument<int>("limit");
                var artists = await artistRepository.GetAllArtistsAsync();
                return artists.Take(limit).ToList();
            }
        );

        FieldAsync<ArtistType>(
            "Artist",
            Description = "Get Artist",
            arguments: new QueryArguments
            {
                new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "artistId"}
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
