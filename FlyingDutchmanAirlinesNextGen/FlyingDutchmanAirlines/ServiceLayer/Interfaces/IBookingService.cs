namespace FlyingDutchmanAirlines.ServiceLayer.Interfaces;

public interface IBookingService
{
    Task<(bool result, Exception? exception)> CreateBooking(string name, int flightNumber);
}
