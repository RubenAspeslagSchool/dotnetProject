using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL;
using System.Reflection;

namespace GraphQL.Types;

public class ArtistType : ObjectGraphType<Artist>
{
    public ArtistType(ICardRepository repository)
    {
        Name = "Artist";
        Field(a => a.Id);
        Field(a => a.FullName);
        Field<ListGraphType<CardType>>(
            "Cards",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> { Name = "limit" }
            },
            resolve: context =>
            {
                int? limit = context.GetArgument<int?>("limit");
                var cards = repository.GetAllCardsByArtistId(context.Source.Id);

                if (limit.HasValue)
                {
                    return cards.Take(limit.Value).ToList();
                }

                return cards.ToList();
            }
        );
    }
}
