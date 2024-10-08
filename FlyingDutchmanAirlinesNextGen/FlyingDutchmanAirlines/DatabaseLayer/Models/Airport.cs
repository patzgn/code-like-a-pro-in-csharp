namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Airport
{
    public int AirportId { get; set; }
    public required string City { get; set; }
    public required string Iata { get; set; }

    public ICollection<Flight> FlightDestinationNavigation { get; set; }
    public ICollection<Flight> FlightOriginNavigation { get; set; }

    public Airport()
    {
        FlightDestinationNavigation = new HashSet<Flight>();
        FlightOriginNavigation = new HashSet<Flight>();
    }
}
