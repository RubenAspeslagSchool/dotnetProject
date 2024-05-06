using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLAPI.GraphQLTypes;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.Filters;
using Microsoft.EntityFrameworkCore;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<MtgV1Context>
    (options => options.UseSqlServer(config.GetConnectionString("MtgDb")));

builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<IArtistRepository, SqlArtistRepository>();

builder.Services.AddScoped<RootSchema>();
builder.Services.AddGraphQL()
                .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
                .AddDataLoader()
                .AddSystemTextJson();

builder.Services.Configure<PaginationFilter>(config.GetSection("Paging"));




WebApplication app = builder.Build();
app.UseGraphQL<RootSchema>();
app.UseGraphQLPlayground(
    "/ui/playground",
    new PlaygroundOptions()
    {
        EditorTheme = EditorTheme.Light
    });



app.Run();
