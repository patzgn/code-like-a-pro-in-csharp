using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAirportId, int destinationAirportId)
    {
        if (!originAirportId.IsPositive() || !destinationAirportId.IsPositive())
        {
            Console.WriteLine(
                "Argument Exception in GetFlightByFlightNumber!"
                + $" OriginAirportId = {originAirportId}, destinationAirportId = {destinationAirportId}");
            throw new ArgumentException("Invalid arguments provided");
        }

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
