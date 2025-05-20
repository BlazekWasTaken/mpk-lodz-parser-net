namespace mpk_lodz_parser_net.Model;

public class Schedule
{
    public TimeOnly Time { get; set; }
    public string? Stop { get; set; }
    public string? Comment { get; set; }
    public List<Departure> Departures { get; } = [];
}