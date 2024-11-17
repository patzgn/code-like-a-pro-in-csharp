using System.Net;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("[controller]")]
public class BookingController(IBookingService bookingService) : Controller
{
    [HttpPost("{flightNumber:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateBooking([FromBody] BookingData body, int flightNumber)
    {
        if (!ModelState.IsValid || !flightNumber.IsPositive())
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ModelState.Root.Errors.First().ErrorMessage);
        }

        var name = $"{body.FirstName} {body.LastName}";
        var (result, exception) = await bookingService.CreateBooking(name, flightNumber);

        if (result && exception is null)
        {
            return StatusCode((int)HttpStatusCode.Created);
        }

        return exception is CouldNotAddBookingToDatabaseException
            ? StatusCode((int)HttpStatusCode.NotFound)
            : StatusCode((int)HttpStatusCode.InternalServerError, exception?.Message);
    }
}
