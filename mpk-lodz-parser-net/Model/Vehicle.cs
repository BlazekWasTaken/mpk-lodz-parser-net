namespace mpk_lodz_parser_net.Model;

public class Vehicle
{
    public int Id { get; set; }
    public string Number { get; set; }
    public VehicleType Type { get; set; }
}

public enum VehicleType
{
    Bus = 0,
    Tram = 1
}