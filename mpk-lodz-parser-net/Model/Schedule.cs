namespace mpk_lodz_parser_net.Model;

public class Schedule
{
    public TimeOnly Time { get; set; }
    public int StopNum { get; set; }
    public string? StopName { get; set; }
    public string? Comment { get; set; }
    public List<Departure> Departures { get; } = [];
}