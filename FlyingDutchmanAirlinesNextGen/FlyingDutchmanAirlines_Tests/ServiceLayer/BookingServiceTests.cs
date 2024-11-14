using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class BookingServiceTests
{
    private Mock<IBookingRepository> _mockBookingRepository = default!;
    private Mock<ICustomerRepository> _mockCustomerRepository = default!;
    private Mock<IFlightRepository> _mockFlightRepository = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockFlightRepository = new Mock<IFlightRepository>();
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0))
            .Returns(Task.CompletedTask);
        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Leo Tolstoy"))
            .Returns(Task.FromResult(new Customer("Leo Tolstoy")));
        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(0))
            .Returns(Task.FromResult(new Flight()));

        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking("Leo Tolstoy", 0);

        Assert.IsTrue(result);
        Assert.IsNull(exception);
    }

    [TestMethod]
    public async Task CreateBooking_Success_CustomerNotInDatabase()
    {
        _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0))
            .Returns(Task.CompletedTask);
        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Konrad Zuse"))
            .Throws(new CustomerNotFoundException());

        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking("Konrad Zuse", 0);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CustomerNotFoundException));
    }

    [TestMethod]
    [DataRow("", 0)]
    [DataRow(null, 0)]
    [DataRow("Galileo Galilei", -1)]
    public async Task CreateBooking_Failure_InvalidInputArguments(string name, int flightNumber)
    {
        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking(name, flightNumber);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public async Task CreateBooking_Failure_RepositoryException_ArgumentException()
    {
        _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 1))
            .Throws(new ArgumentException());

        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Galileo Galilei"))
            .Returns(Task.FromResult(new Customer("Galileo Galilei") { CustomerId = 0 }));

        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(1))
            .Returns(Task.FromResult(new Flight()));

        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking("Galileo Galilei", 1);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(ArgumentException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_CouldNotAddBookingToDatabaseException()
    {
        _mockBookingRepository.Setup(repository => repository.CreateBooking(1, 2))
            .Throws(new CouldNotAddBookingToDatabaseException());

        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Eise Eisinga"))
            .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));

        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking("Eise Eisinga", 2);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_FlightNotInDatabase()
    {
        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(1))
            .Throws(new FlightNotFoundException());

        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking("Maurits Escher", 1);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_CustomerNotInDatabase_RepositoryFailure()
    {
        _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0))
            .Throws(new CouldNotAddBookingToDatabaseException());
        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(0))
            .ReturnsAsync(new Flight());
        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Bill Gates"))
            .Returns(Task.FromResult(new Customer("Bill Gates")));

        var service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
            _mockFlightRepository.Object);

        var (result, exception) = await service.CreateBooking("Bill Gates", 0);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }
}
