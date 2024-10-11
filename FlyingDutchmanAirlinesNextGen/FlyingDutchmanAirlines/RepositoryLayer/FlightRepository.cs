using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository : IFlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber)
    {
        if (!flightNumber.IsPositive())
        {
            Console.WriteLine(
                $"Could not find flight in GetFlightByFlightNumber! FlightNumber = {flightNumber}");
            throw new FlightNotFoundException();
        }

        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber)
            ?? throw new FlightNotFoundException();
    }
}
