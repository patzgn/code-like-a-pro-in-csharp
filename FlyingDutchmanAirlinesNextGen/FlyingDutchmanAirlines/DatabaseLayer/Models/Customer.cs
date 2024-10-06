using System;

namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Customer
{
    public Customer(string name)
    {
        Booking = new HashSet<Booking>();
        Name = name;
    }

    public int CustomerId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Booking> Booking { get; set; }
}
