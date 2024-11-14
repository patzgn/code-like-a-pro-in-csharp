using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository(FlyingDutchmanAirlinesContext context) : ICustomerRepository
{
    public async Task<bool> CreateCustomer(string name)
    {
        if (!IsCustomerNameValid(name))
        {
            return false;
        }

        try
        {
            var newCustomer = new Customer(name);
            await using (context)
            {
                context.Customers.Add(newCustomer);
                await context.SaveChangesAsync();
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<Customer> GetCustomerByName(string name)
    {
        if (!IsCustomerNameValid(name))
        {
            Console.WriteLine($"Could not find customer in GetCustomerByName! Name = {name}");
            throw new CustomerNotFoundException();
        }

        return await context.Customers.FirstOrDefaultAsync(c => c.Name == name) ??
               throw new CustomerNotFoundException();
    }

    private bool IsCustomerNameValid(string name)
    {
        char[] forbiddenCharacters = ['!', '@', '#', '$', '%', '&', '*'];

        return !string.IsNullOrEmpty(name) && !name.Any(x => forbiddenCharacters.Contains(x));
    }
}
