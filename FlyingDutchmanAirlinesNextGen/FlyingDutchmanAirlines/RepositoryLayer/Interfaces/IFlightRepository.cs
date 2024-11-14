using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

public interface IFlightRepository
{
    Task<Flight> GetFlightByFlightNumber(int flightNumber);
    Queue<Flight> GetFlights();
}
