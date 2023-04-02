public class Match
{
    public Guid Id { get; set; }
    public Team? Team1 { get; set; }
    public Team? Team2 { get; set; }
    public string? Score { get; set; }
}