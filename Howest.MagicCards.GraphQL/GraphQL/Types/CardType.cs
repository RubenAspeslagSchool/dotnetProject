using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace GraphQLAPI.GraphQLTypes;
public class CardType: ObjectGraphType<Card>
{
    public CardType(IArtistRepository artistRepository)
    {
        Name = "Card";

        Field(c => c.Id, type: typeof(IdGraphType))
            .Description("Id of the card");

        Field(c => c.Name, type: typeof(StringGraphType))
            .Description("Card name");
        Field(c => c.ManaCost, type: typeof(StringGraphType))
            .Description("Mana Cost");
        Field(c => c.Power, type: typeof(StringGraphType));
        Field(c => c.Toughness, type: typeof(StringGraphType));
        Field(c => c.SetCode, type: typeof(StringGraphType));
        Field(c => c.RarityCode, type: typeof(StringGraphType));
        Field<ArtistType>(
            "Artist",
            resolve: context => artistRepository.GetArtistAsync((int) context.Source.Id)
            );
    }
}
