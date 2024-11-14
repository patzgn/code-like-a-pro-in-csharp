using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class FlightServiceTests
{
    private Mock<IFlightRepository> _mockFlightRepository = default!;
    private Mock<IAirportRepository> _mockAirportRepository = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockFlightRepository = new Mock<IFlightRepository>();
        _mockAirportRepository = new Mock<IAirportRepository>();

        _mockAirportRepository.Setup(repository => repository.GetAirportById(31))
            .ReturnsAsync(new Airport
            {
                AirportId = 31,
                City = "Mexico City",
                Iata = "MEX",
            });

        _mockAirportRepository.Setup(repository => repository.GetAirportById(92))
            .ReturnsAsync(new Airport
            {
                AirportId = 92,
                City = "Ulaanbaatar",
                Iata = "UBN",
            });

        var flightInDatabase = new Flight
        {
            FlightNumber = 148,
            Origin = 31,
            Destination = 92,
        };

        var mockReturn = new Queue<Flight>(1);
        mockReturn.Enqueue(flightInDatabase);

        _mockFlightRepository.Setup(repository => repository.GetFlights()).Returns(mockReturn);
        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(148))
            .Returns(Task.FromResult(flightInDatabase));
    }

    [TestMethod]
    public async Task GetFlights_Success()
    {
        var service = new FlightService(_mockFlightRepository.Object, _mockAirportRepository.Object);

        await foreach (var flightView in service.GetFlights())
        {
            Assert.IsNotNull(flightView);
            Assert.AreEqual(flightView.FlightNumber, "148");
            Assert.AreEqual(flightView.Origin.City, "Mexico City");
            Assert.AreEqual(flightView.Origin.Code, "MEX");
            Assert.AreEqual(flightView.Destination.City, "Ulaanbaatar");
            Assert.AreEqual(flightView.Destination.Code, "UBN");
        }
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlights_Failure_RepositoryException()
    {
        _mockAirportRepository.Setup(repository => repository.GetAirportById(31))
            .ThrowsAsync(new FlightNotFoundException());

        var service = new FlightService(_mockFlightRepository.Object, _mockAirportRepository.Object);
        await foreach (var _ in service.GetFlights())
        {
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlights_Failure_RegularException()
    {
        _mockAirportRepository.Setup(repository => repository.GetAirportById(31))
            .ThrowsAsync(new NullReferenceException());

        var service = new FlightService(_mockFlightRepository.Object, _mockAirportRepository.Object);
        await foreach (var _ in service.GetFlights())
        {
        }
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        var service = new FlightService(_mockFlightRepository.Object, _mockAirportRepository.Object);

        var flightView = await service.GetFlightByFlightNumber(148);

        Assert.IsNotNull(flightView);
        Assert.AreEqual(flightView.FlightNumber, "148");
        Assert.AreEqual(flightView.Origin.City, "Mexico City");
        Assert.AreEqual(flightView.Origin.Code, "MEX");
        Assert.AreEqual(flightView.Destination.City, "Ulaanbaatar");
        Assert.AreEqual(flightView.Destination.Code, "UBN");
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_RepositoryException_FlightNotFoundException()
    {
        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(-1))
            .Throws(new FlightNotFoundException());

        var service = new FlightService(_mockFlightRepository.Object, _mockAirportRepository.Object);

        await service.GetFlightByFlightNumber(-1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlightByFlightNumber_Failure_RepositoryException_Exception()
    {
        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(-1))
            .ThrowsAsync(new OverflowException());
        
        var service = new FlightService(_mockFlightRepository.Object, _mockAirportRepository.Object);
        
        await service.GetFlightByFlightNumber(-1);
    }
}
