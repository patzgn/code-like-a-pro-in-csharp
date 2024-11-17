using System.Net;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer.Interfaces;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("[controller]")]
public class FlightController(IFlightService flightService) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFlights()
    {
        try
        {
            var flights = new Queue<FlightView>();
            await foreach (var flight in flightService.GetFlights())
            {
                flights.Enqueue(flight);
            }

            return StatusCode((int)HttpStatusCode.OK, flights);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int)HttpStatusCode.NotFound, "No flights were found in the database");
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
        }
    }

    [HttpGet("{flightNumber:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            if (!flightNumber.IsPositive())
            {
                throw new Exception();
            }

            var flight = await flightService.GetFlightByFlightNumber(flightNumber);

            return StatusCode((int)HttpStatusCode.OK, flight);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int)HttpStatusCode.NotFound, "The flight was not found in the database");
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, "Bad request");
        }
    }
}
