using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Airport> GetAirportById(int airportId)
    {
        if (!airportId.IsPositive())
        {
            Console.WriteLine(
                $"Argument Exception in GetAirportById! AirportId = {airportId}");
            throw new ArgumentException("Invalid arguments provided");
        }

        return await _context.Airports.FirstOrDefaultAsync(x => x.AirportId == airportId)
            ?? throw new AirportNotFoundException();
    }
}
