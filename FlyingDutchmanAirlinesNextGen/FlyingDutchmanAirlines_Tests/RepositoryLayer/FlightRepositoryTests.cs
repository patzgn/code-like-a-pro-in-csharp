using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class FlightRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context = default!;
    private FlightRepository _repository = default!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchman")
            .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        var testFlight = new Flight
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 2,
        };

        var testFlight2 = new Flight
        {
            FlightNumber = 10,
            Origin = 3,
            Destination = 4,
        };

        _context.Flights.Add(testFlight);
        _context.Flights.Add(testFlight2);
        await _context.SaveChangesAsync();

        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        var flight = await _repository.GetFlightByFlightNumber(1);
        Assert.IsNotNull(flight);

        var dbFlight = _context.Flights.First(f => f.FlightNumber == 1);
        Assert.IsNotNull(dbFlight);

        Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
        Assert.AreEqual(dbFlight.Origin, flight.Origin);
        Assert.AreEqual(dbFlight.Destination, flight.Destination);
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber()
    {
        await _repository.GetFlightByFlightNumber(-1);
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_DatabaseException()
    {
        await _repository.GetFlightByFlightNumber(2);
    }

    [TestMethod]
    public void GetFlights_Success()
    {
        var flights = _repository.GetFlights();
        Assert.IsNotNull(flights);

        var dbFlight = _context.Flights.First(f => f.FlightNumber == 1);
        Assert.IsNotNull(dbFlight);

        Assert.AreEqual(dbFlight.FlightNumber, flights.Peek().FlightNumber);
        Assert.AreEqual(dbFlight.Origin, flights.Peek().Origin);
        Assert.AreEqual(dbFlight.Destination, flights.Peek().Destination);
    }
}
