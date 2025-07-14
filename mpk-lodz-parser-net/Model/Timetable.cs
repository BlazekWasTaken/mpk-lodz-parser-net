using System.ComponentModel.DataAnnotations;
using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.Model;

public class Timetable
{
    public Stop Stop { get; set; }
    public Vehicle Vehicle { get; set; }
    public ICollection<TimetableSubroute> Subroutes { get; } = [];
}