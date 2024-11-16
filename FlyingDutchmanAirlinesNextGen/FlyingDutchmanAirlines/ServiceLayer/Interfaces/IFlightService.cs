using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer.Interfaces;

public interface IFlightService
{
    IAsyncEnumerable<FlightView> GetFlights();
    Task<FlightView> GetFlightByFlightNumber(int flightNumber);
}
