namespace mpk_lodz_parser_net.Model;

public class Stop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public List<StopManager> Stops { get; set; } = [];
    public List<Vehicle> Vehicles { get; set; } = [];
}