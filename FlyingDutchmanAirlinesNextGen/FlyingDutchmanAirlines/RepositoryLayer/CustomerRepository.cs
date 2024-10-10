using System.Reflection;
using System.Runtime.CompilerServices;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public CustomerRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public CustomerRepository()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
        {
            throw new Exception("This constructor should only by used for testing");
        }
    }

    public async Task<bool> CreateCustomer(string name)
    {
        if (!IsCusomerNameValid(name))
        {
            return false;
        }

        try
        {
            Customer newCustomer = new Customer(name);
            using (_context)
            {
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public virtual async Task<Customer> GetCustomerByName(string name)
    {
        if (!IsCusomerNameValid(name))
        {
            Console.WriteLine(
                $"Could not find customer in GetCustomerByName! Name = {name}");
            throw new CustomerNotFoundException();
        }

        return await _context.Customers.FirstOrDefaultAsync(c => c.Name == name)
            ?? throw new CustomerNotFoundException();
    }

    private bool IsCusomerNameValid(string name)
    {
        char[] forbiddenCharacters = ['!', '@', '#', '$', '%', '&', '*'];

        return !string.IsNullOrEmpty(name)
               && !name.Any(x => forbiddenCharacters.Contains(x));
    }
}
