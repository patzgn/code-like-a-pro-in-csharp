using System.Runtime.ExceptionServices;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IFlightRepository _flightRepository;

    public BookingService(IBookingRepository bookingRepository, ICustomerRepository customerRepository,
        IFlightRepository flightRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _flightRepository = flightRepository;
    }

    public async Task<(bool result, Exception? exception)> CreateBooking(string name, int flightNumber)
    {
        if (string.IsNullOrEmpty(name) || !flightNumber.IsPositive())
        {
            return (false, new ArgumentException());
        }

        try
        {
            Customer customer = await GetCustomerFromDatabase(name)
                ?? await AddCustomerToDatabase(name);

            if (!await FlightExistsInDatabase(flightNumber))
            {
                return (false, new CouldNotAddBookingToDatabaseException());
            }

            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
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
            return await _customerRepository.GetCustomerByName(name);
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
        await _customerRepository.CreateCustomer(name);
        return await _customerRepository.GetCustomerByName(name);
    }

    private async Task<bool> FlightExistsInDatabase(int flightNumber)
    {
        try
        {
            return await _flightRepository.GetFlightByFlightNumber(flightNumber) != null;
        }
        catch (FlightNotFoundException)
        {
            return false;
        }
    }
}
