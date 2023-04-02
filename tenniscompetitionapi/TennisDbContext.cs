using Microsoft.EntityFrameworkCore;

public class TennisDbContext : DbContext
{
public TennisDbContext(DbContextOptions<TennisDbContext> options) : base(options)
{
}

public DbSet<Player>? Players { get; set; }
}
