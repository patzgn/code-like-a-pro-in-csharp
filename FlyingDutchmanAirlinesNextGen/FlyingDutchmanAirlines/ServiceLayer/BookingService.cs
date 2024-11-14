using System.Runtime.ExceptionServices;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService(
    IBookingRepository bookingRepository,
    ICustomerRepository customerRepository,
    IFlightRepository flightRepository)
{
    public async Task<(bool result, Exception? exception)> CreateBooking(string name, int flightNumber)
    {
        if (string.IsNullOrEmpty(name) || !flightNumber.IsPositive())
        {
            return (false, new ArgumentException());
        }

        try
        {
            var customer = await GetCustomerFromDatabase(name) ?? await AddCustomerToDatabase(name);

            if (!await FlightExistsInDatabase(flightNumber))
            {
                return (false, new CouldNotAddBookingToDatabaseException());
            }

            await bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }
    }

    private async Task<Customer?> GetCustomerFromDatabase(string name)
    {
        try
        {
            return await customerRepository.GetCustomerByName(name);
        }
        catch (CustomerNotFoundException)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException ?? new Exception()).Throw();
            return null;
        }
    }

    private async Task<Customer> AddCustomerToDatabase(string name)
    {
        await customerRepository.CreateCustomer(name);
        return await customerRepository.GetCustomerByName(name);
    }

    private async Task<bool> FlightExistsInDatabase(int flightNumber)
    {
        try
        {
            await flightRepository.GetFlightByFlightNumber(flightNumber);
        }
        catch (FlightNotFoundException)
        {
            return false;
        }

        return true;
    }
}
