using mpk_lodz_parser_net.infrastructure.Model;

namespace mpk_lodz_parser_net.Model;

public class Departure
{
    public Vehicle Vehicle { get; set; }
    public DateTime When { get; set; }
    public string? WhenText { get; set; }
    public string? Direction { get; set; }
}