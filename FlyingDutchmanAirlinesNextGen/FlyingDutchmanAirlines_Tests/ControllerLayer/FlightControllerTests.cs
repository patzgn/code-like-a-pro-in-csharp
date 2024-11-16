using System.Net;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer.Interfaces;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

[TestClass]
public class FlightControllerTests
{
    [TestMethod]
    public async Task GetFlights_Success()
    {
        var flightService = new Mock<IFlightService>();

        var returnFlightViews = new List<FlightView>(2)
        {
            new FlightView("1932", ("Groningen", "GRQ"), ("Phoenix", "PHX")),
            new FlightView("841", ("New York City", "JFK"), ("London", "LHR")),
        };

        flightService.Setup(s => s.GetFlights()).Returns(FlightViewAsyncGenerator(returnFlightViews));

        var controller = new FlightController(flightService.Object);
        var response = await controller.GetFlights() as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

        var content = response.Value as Queue<FlightView>;
        Assert.IsNotNull(content);

        Assert.IsTrue(returnFlightViews.All(flight => content.Contains(flight)));
    }

    [TestMethod]
    public async Task GetFlights_Failure_FlightNotFoundException_404()
    {
        var flightService = new Mock<IFlightService>();

        flightService.Setup(s => s.GetFlights()).Throws(new FlightNotFoundException());

        var controller = new FlightController(flightService.Object);
        var response = await controller.GetFlights() as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual("No flights were found in the database", response.Value);
    }

    [TestMethod]
    public async Task GetFlights_Failure_ArgumentException_500()
    {
        var flightService = new Mock<IFlightService>();

        flightService.Setup(s => s.GetFlights()).Throws(new ArgumentException());

        var controller = new FlightController(flightService.Object);
        var response = await controller.GetFlights() as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.AreEqual("An error occurred", response.Value);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        var flightService = new Mock<IFlightService>();

        var returnedFlightView = new FlightView("0", ("Lagos", "LOS"), ("Marrakesh", "RAK"));

        flightService.Setup(s => s.GetFlightByFlightNumber(0)).Returns(Task.FromResult(returnedFlightView));

        var controller = new FlightController(flightService.Object);
        var response = await controller.GetFlightByFlightNumber(0) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

        var content = response.Value as FlightView;
        Assert.IsNotNull(content);

        Assert.AreEqual(returnedFlightView, content);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Failure_FlightNotFoundException_404()
    {
        var flightService = new Mock<IFlightService>();

        flightService.Setup(s => s.GetFlightByFlightNumber(1)).Throws(new FlightNotFoundException());

        var controller = new FlightController(flightService.Object);
        var response = await controller.GetFlightByFlightNumber(1) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual("The flight was not found in the database", response.Value);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(1)]
    public async Task GetFlightByFlightNumber_Failure_ArgumentException_400(int flightNumber)
    {
        var flightService = new Mock<IFlightService>();

        flightService.Setup(s => s.GetFlightByFlightNumber(1)).Throws(new ArgumentException());

        var controller = new FlightController(flightService.Object);
        var response = await controller.GetFlightByFlightNumber(flightNumber) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual("Bad request", response.Value);
    }

    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(IEnumerable<FlightView> views)
    {
        foreach (var flightView in views)
        {
            yield return flightView;
        }
    }
}
