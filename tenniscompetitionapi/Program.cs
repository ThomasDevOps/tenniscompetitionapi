using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TennisCompetitionApi", Version = "v1" });
});

builder.Services.AddDbContext<TennisDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TennisDb"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World Thomas!");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TennisCompetitionApi v1"));

app.Run();
