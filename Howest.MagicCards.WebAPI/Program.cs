using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using Type = System.Type;
using FluentValidation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "magicCards webApi V1.1",
        Version = "v1.1",
        Description = "version 1.1"
    });

    c.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "magicCards webApi V1.5",
        Version = "v1.5",
        Description = "version 1.5"
    });
});
builder.Services.AddDbContext<MtgV1Context>(options => options.UseSqlServer(config.GetConnectionString("MtgDb")));

builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<IRarityRepository, SqlRarityRepository>();
builder.Services.AddAutoMapper(new Type[] {
    typeof(Howest.MagicCards.Shared.Mapping.DecksProfile)
});
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    // options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.DefaultApiVersion = new ApiVersion(1, 1);
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    }
);

builder.Services.AddResponseCaching();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Magiccards web API V1.1");
        c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "Magiccards web API V1.5");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();
app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "v1_1",
        pattern: "api/V1.1/{controller=Cards}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "v1_5",
        pattern: "api/V1.5/{controller=Cards}/{action=Index}/{id?}");
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "V1.1");
    c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "V1.5");
});

app.Run();