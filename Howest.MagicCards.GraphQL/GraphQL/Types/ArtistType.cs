using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace GraphQLAPI.GraphQLTypes;

public class ArtistType: ObjectGraphType<Artist>
{
    public  ArtistType(ICardRepository repository)
    {
        Name = "Artist";
        Field(a => a.Id);
        Field(a => a.FullName);
        Field<ListGraphType<CardType>>
            (
                "Cards",
                arguments: new QueryArguments
                {
                    new QueryArgument<IntGraphType> { Name = "limit", DefaultValue = 10 },
                },
                resolve: context =>
                {
                    int limit = context.GetArgument<int>("limit");

                    return  repository.GetAllCardsByArtistId(context.Source.Id).Take(limit).ToList();
                }
            );
    }
}

