using FlyingDutchmanAirlines.ControllerLayer.JsonData;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer.JsonData;

[TestClass]
public class BookingDataTests
{
    [TestMethod]
    public void BookingData_ValidData()
    {
        var bookingData = new BookingData
        {
            FirstName = "Marina",
            LastName = "Michaels",
        };

        Assert.AreEqual("Marina", bookingData.FirstName);
        Assert.AreEqual("Michaels", bookingData.LastName);
    }

    [TestMethod]
    [DataRow("Mike", null)]
    [DataRow(null, "Morand")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BookingData_InvalidData_NullPointers(string firstName, string lastName)
    {
        var bookingData = new BookingData
        {
            FirstName = firstName,
            LastName = lastName
        };
    }

    [TestMethod]
    [DataRow("Eleonor", "")]
    [DataRow("", "Wilke")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BookingData_InvalidData_EmptyStrings(string firstName, string lastName)
    {
        var bookingData = new BookingData
        {
            FirstName = firstName,
            LastName = lastName
        };
    }
}
