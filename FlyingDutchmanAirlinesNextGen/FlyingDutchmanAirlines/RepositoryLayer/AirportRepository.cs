using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository(FlyingDutchmanAirlinesContext context) : IAirportRepository
{
    public async Task<Airport> GetAirportById(int airportId)
    {
        if (!airportId.IsPositive())
        {
            Console.WriteLine($"Argument Exception in GetAirportById! AirportId = {airportId}");
            throw new ArgumentException("Invalid arguments provided");
        }

        return await context.Airports.FirstOrDefaultAsync(x => x.AirportId == airportId) ??
               throw new AirportNotFoundException();
    }
}
