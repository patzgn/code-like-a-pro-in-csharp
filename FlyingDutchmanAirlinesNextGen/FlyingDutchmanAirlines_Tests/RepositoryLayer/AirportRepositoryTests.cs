using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class AirportRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context = default!;
    private AirportRepository _repository = default!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchman")
            .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        var testAirports = new SortedList<string, Airport>
        {
            {
                "GOH",
                new Airport
                {
                    AirportId = 0,
                    City = "Nuuk",
                    Iata = "GOH"
                }
            },
            {
                "PHX",
                new Airport
                {
                    AirportId = 1,
                    City = "Phoenix",
                    Iata = "PHX"
                }
            },
            {
                "DDH",
                new Airport
                {
                    AirportId = 2,
                    City = "Bennington",
                    Iata = "DDH"
                }
            },
            {
                "RDU",
                new Airport
                {
                    AirportId = 3,
                    City = "Raleigh-Durham",
                    Iata = "RDU"
                }
            }
        };

        _context.Airports.AddRange(testAirports.Values);
        await _context.SaveChangesAsync();

        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public async Task GetAirportById_Success(int airportId)
    {
        var airport = await _repository.GetAirportById(airportId);
        Assert.IsNotNull(airport);

        var dbAirport = _context.Airports.First(a => a.AirportId == airportId);
        Assert.AreEqual(dbAirport.AirportId, airport.AirportId);
        Assert.AreEqual(dbAirport.City, airport.City);
        Assert.AreEqual(dbAirport.Iata, airport.Iata);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetAirportById_Failure_InvalidInput()
    {
        var outputStream = new StringWriter();
        try
        {
            Console.SetOut(outputStream);
            await _repository.GetAirportById(-1);
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(outputStream.ToString()
                .Contains("Argument Exception in GetAirportById! AirportId = -1"));
            throw;
        }
        finally
        {
            await outputStream.DisposeAsync();
        }
    }

    [TestMethod]
    [ExpectedException(typeof(AirportNotFoundException))]
    public async Task GetAirportById_Failure_DatabaseException()
    {
        await _repository.GetAirportById(10);
    }
}
