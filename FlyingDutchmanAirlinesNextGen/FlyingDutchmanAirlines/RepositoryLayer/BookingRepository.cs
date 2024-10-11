using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository : IBookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public BookingRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task CreateBooking(int customerId, int flightNumber)
    {
        if (!customerId.IsPositive() || !flightNumber.IsPositive())
        {
            Console.WriteLine(
                $"Argument Exception in CreateBooking! CustomerId = {customerId}, flightNumber = {flightNumber}");
            throw new ArgumentException("Invalid arguments provided");
        }

        Booking newBooking = new Booking
        {
            CustomerId = customerId,
            FlightNumber = flightNumber,
        };

        try
        {
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during database query: {ex.Message}");
            throw new CouldNotAddBookingToDatabaseException();
        }
    }
}
