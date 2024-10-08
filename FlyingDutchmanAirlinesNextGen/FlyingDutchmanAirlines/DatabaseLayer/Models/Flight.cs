namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Flight
{
    public int FlightNumber { get; set; }
    public int Origin { get; set; }
    public int Destination { get; set; }

    public Airport DestinationNavigation { get; set; } = default!;
    public Airport OriginNavigation { get; set; } = default!;
    public ICollection<Booking> Booking { get; set; }

    public Flight()
    {
        Booking = new HashSet<Booking>();
    }
}
