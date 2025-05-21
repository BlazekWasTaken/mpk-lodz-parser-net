using System.ComponentModel.DataAnnotations;

namespace mpk_lodz_parser_net.infrastructure.Model;

public class Vehicle
{
    [Key]
    public int Id { get; set; }
    public string Number { get; set; }
    public VehicleType Type { get; set; }
    public ICollection<Stop> Stops { get; } = [];
}

public enum VehicleType
{
    Bus = 0,
    Tram = 1
}