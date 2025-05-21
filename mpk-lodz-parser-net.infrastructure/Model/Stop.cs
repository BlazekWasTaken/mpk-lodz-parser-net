using System.ComponentModel.DataAnnotations;

namespace mpk_lodz_parser_net.infrastructure.Model;

public class Stop
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public ICollection<Vehicle> Vehicles { get; } = [];
}