using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository(FlyingDutchmanAirlinesContext context) : IFlightRepository
{
    public async Task<Flight> GetFlightByFlightNumber(int flightNumber)
    {
        if (!flightNumber.IsPositive())
        {
            Console.WriteLine($"Could not find flight in GetFlightByFlightNumber! FlightNumber = {flightNumber}");
            throw new FlightNotFoundException();
        }

        return await context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ??
               throw new FlightNotFoundException();
    }

    public Queue<Flight> GetFlights()
    {
        var flights = new Queue<Flight>();
        foreach (var flight in context.Flights)
        {
            flights.Enqueue(flight);
        }

        return flights;
    }
}
