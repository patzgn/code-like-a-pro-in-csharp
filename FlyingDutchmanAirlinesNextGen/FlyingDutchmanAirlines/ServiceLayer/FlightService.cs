using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using FlyingDutchmanAirlines.ServiceLayer.Interfaces;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class FlightService(IFlightRepository flightRepository, IAirportRepository airportRepository) : IFlightService
{
    public async IAsyncEnumerable<FlightView> GetFlights()
    {
        var flights = flightRepository.GetFlights();
        foreach (var flight in flights)
        {
            Airport originAirport;
            Airport destinationAirport;

            try
            {
                originAirport = await airportRepository.GetAirportById(flight.Origin);
                destinationAirport = await airportRepository.GetAirportById(flight.Destination);
            }
            catch (FlightNotFoundException)
            {
                throw new FlightNotFoundException();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            yield return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata),
                (destinationAirport.City, destinationAirport.Iata));
        }
    }

    public async Task<FlightView> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            var flight = await flightRepository.GetFlightByFlightNumber(flightNumber);
            var originAirport = await airportRepository.GetAirportById(flight.Origin);
            var destinationAirport = await airportRepository.GetAirportById(flight.Destination);

            return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata),
                (destinationAirport.City, destinationAirport.Iata));
        }
        catch (FlightNotFoundException)
        {
            throw new FlightNotFoundException();
        }
        catch (Exception)
        {
            throw new ArgumentException();
        }
    }
}
