using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TennisCompetitionApi", Version = "v1" });
});

builder.Services.AddDbContext<TennisDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MYSQLCONNSTR_TennisDb"));
});


var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<TennisDbContext>();
dbContext.Database.Migrate();

app.MapGet("/players", () => dbContext.Players);
app.MapGet("/players/{id}", (Guid id) => dbContext.Players?.Find(id));
app.MapPost("/players", (Player player) =>
{
    player.Id = Guid.NewGuid();
    dbContext.Players.Add(player);
    dbContext.SaveChanges();
    return Results.Created($"/players/{player.Id}", player);
});
app.MapPut("/players/{id}", (Player player, Guid id) =>
{
    var existingPlayer = dbContext.Players?.Find(id);
    existingPlayer.Name = player.Name;
    dbContext.SaveChanges();
    return Results.NoContent();
});
app.MapDelete("/players/{id}", (Guid id) =>
{
    var existingPlayer = dbContext.Players.Find(id);
    dbContext.Players.Remove(existingPlayer);
    dbContext.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/teams", () => dbContext.Teams);
app.MapGet("/teams/{id}", (Guid id) => dbContext.Teams?.Find(id));
app.MapPost("/teams", (Team team) =>
{
    dbContext.Teams?.Add(team);
    dbContext.SaveChanges();
    return Results.Created($"/teams/{team.Id}", team);
});
app.MapPut("/teams/{id}", (Team team, Guid id) =>
{
    var existingTeam = dbContext.Teams?.Find(id);
    existingTeam.Player1 = team.Player1;
    existingTeam.Player2 = team.Player2;
    dbContext.SaveChanges();
    return Results.NoContent();
});
app.MapDelete("/teams/{id}", (Guid id) =>
{
    var existingTeam = dbContext.Teams?.Find(id);
    dbContext.Teams.Remove(existingTeam);
    dbContext.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/matches", () => dbContext.Matches);
app.MapGet("/matches/{id}", (Guid id) => dbContext.Matches?.Find(id));
app.MapPost("/matches", (Match match) =>
{
    dbContext.Matches?.Add(match);
    dbContext.SaveChanges();
    return Results.Created($"/matches/{match.Id}", match);
});
app.MapPut("/matches/{id}", (Match match, Guid id) =>
{
    var existingMatch = dbContext.Matches?.Find(id);
    existingMatch.Team1 = match.Team1;
    existingMatch.Team2 = match.Team2;
    dbContext.SaveChanges();
    return Results.NoContent();
});
app.MapDelete("/matches/{id}", (Guid id) =>
{
    var existingMatch = dbContext.Matches?.Find(id);
    dbContext.Matches.Remove(existingMatch);
    dbContext.SaveChanges();
    return Results.NoContent();
});

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TennisCompetitionApi v1"));

app.Run();
