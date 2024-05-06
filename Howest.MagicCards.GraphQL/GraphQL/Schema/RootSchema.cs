using GraphQL.Types;

namespace GraphQLAPI.GraphQLTypes
{
    public class RootSchema: Schema
    {
        public RootSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
        }

    }
}
