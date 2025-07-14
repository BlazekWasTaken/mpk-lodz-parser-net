namespace mpk_lodz_parser_net.Model;

public class TimetableSubroute
{
    public int SubrouteId { get; set; }
    public string Name { get; set; }
    public string Overview { get; set; }
    public ICollection<TimeOnly> TimesOnWeekdays { get; } = [];
    public ICollection<TimeOnly> TimesOnSaturdays { get; } = [];
    public ICollection<TimeOnly> TimesOnSundays { get; } = [];
}