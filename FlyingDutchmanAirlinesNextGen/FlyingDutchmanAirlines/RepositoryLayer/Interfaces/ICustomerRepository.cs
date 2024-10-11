using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer.Interfaces;

public interface ICustomerRepository
{
    Task<bool> CreateCustomer(string name);
    Task<Customer> GetCustomerByName(string name);
}
