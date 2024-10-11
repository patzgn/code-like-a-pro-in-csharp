using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

public interface IAirportRepository
{
    Task<Airport> GetAirportById(int airportId);
}
