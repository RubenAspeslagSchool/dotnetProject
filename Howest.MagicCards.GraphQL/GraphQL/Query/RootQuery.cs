using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Filters;
using Microsoft.Extensions.Options;

namespace GraphQLAPI.GraphQLTypes;

public class RootQuery : ObjectGraphType
{
    public RootQuery(ICardRepository cardRepository,
        IArtistRepository artistRepository,
        IOptions<PaginationFilter> pagingFilter)
    {
        Name = "Query";
        PaginationFilter pagingOptions = pagingFilter.Value;

        #region Cards
        Field<ListGraphType<CardType>>(
            "Cards",
            Description = "Get all cards",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> { Name = "page", DefaultValue = pagingOptions.PageNumber },
            },
            resolve: context =>
            {
                int page = context.GetArgument<int>("page");

                return cardRepository.GetAllCards()
                .ToPagedList(page, pagingOptions.PageSize)
                .ToList();
            }
        );
        #endregion

        #region Artists
        Field<ListGraphType<ArtistType>>(
            "Artists",
            Description = "Get All Artists",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> {Name = "limit", DefaultValue = pagingOptions.PageSize}
            },
            resolve: context =>
            {
                int limit = context.GetArgument<int>("limit");

                return artistRepository.GetAllArtists().Take(limit).ToList();
            }
        );

        Field<ArtistType>(
            "Artist",
            Description = "Get Artist",
            arguments: new QueryArguments
            {
                new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "artistId"}
            },
            resolve: context =>
            {
                int artistId = context.GetArgument<int>("artistId");

                return artistRepository.GetArtist(artistId);
            }
        );
        #endregion
    }
}

