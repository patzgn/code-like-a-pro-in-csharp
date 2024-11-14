using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository(FlyingDutchmanAirlinesContext context) : IBookingRepository
{
    public async Task CreateBooking(int customerId, int flightNumber)
    {
        if (!customerId.IsPositive() || !flightNumber.IsPositive())
        {
            Console.WriteLine(
                $"Argument Exception in CreateBooking! CustomerId = {customerId}, flightNumber = {flightNumber}");
            throw new ArgumentException("Invalid arguments provided");
        }

        var newBooking = new Booking
        {
            CustomerId = customerId,
            FlightNumber = flightNumber,
        };

        try
        {
            context.Bookings.Add(newBooking);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during database query: {ex.Message}");
            throw new CouldNotAddBookingToDatabaseException();
        }
    }
}
