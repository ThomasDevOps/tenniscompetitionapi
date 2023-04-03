using Microsoft.AspNetCore.Authorization;

public static class ContractRelatieEndpoints
{
    public static void MapAllEndpoints(this WebApplication app, TennisDbContext dbContext)
    {
        app.MapGet("/players", [Authorize] () => dbContext.Players);
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
    }
}