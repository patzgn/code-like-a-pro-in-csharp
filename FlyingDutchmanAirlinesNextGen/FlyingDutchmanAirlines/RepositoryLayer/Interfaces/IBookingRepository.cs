namespace FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

public interface IBookingRepository
{
    Task CreateBooking(int customerId, int flightNumber);
}
