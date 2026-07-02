using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models;
using BookingProject.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class CustomerService(AppDbContext context)
{
    public List<Customer>? Get()
    {
        var customers = context.Customers
            .AsNoTracking()
            .ToList();

        if (customers.Count == 0)
        {
            return null;
        }

        return customers;
    }

    public Customer? Get(Guid customerId)
    {
         Customer ? customerToFetch=context.Customers
                   .AsNoTracking()
                   .Include(customer => customer.Bookings)
                   .FirstOrDefault(customer => customer.Id == customerId);

         return customerToFetch !;
    }

    public Guid? GetId(CustomerRequestDto customerRequest)
    {
        ArgumentNullException.ThrowIfNull(customerRequest);

        return context.Customers
            .AsNoTracking()
            .Where(customer =>
                customer.FirstName == customerRequest.FirstName &&
                customer.LastName == customerRequest.LastName)
            .Select(customer => (Guid?)customer.Id)
            .FirstOrDefault();
    }

    public int Add(CustomerRequestDto customerRequestDto)
    {
        if (string.IsNullOrEmpty(customerRequestDto.LastName) ||
            string.IsNullOrEmpty(customerRequestDto.Email) ||
            string.IsNullOrEmpty(customerRequestDto.PhoneNumber) ||
            string.IsNullOrEmpty(customerRequestDto.FirstName)
           )
        {
            throw new CustomExceptions.InvalidCustomerData();
        }
        
        Customer customer = new Customer();
        
        customer.Id = Guid.NewGuid();
        customer.FirstName = customerRequestDto.FirstName;
        customer.LastName = customerRequestDto.LastName;
        customer.Email = customerRequestDto.Email;
        customer.PhoneNumber = customerRequestDto.PhoneNumber;
        
        context.Customers.Add(customer);

        
        return context.SaveChanges();
    }

    public int Update(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        var existingCustomer = context.Customers
                                   .FirstOrDefault(c => c.Id == customer.Id)
                               ?? throw new Exception("Customer not found");

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.Email = customer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber;
        existingCustomer.PersonalNumber = customer.PersonalNumber;

        return context.SaveChanges();
    }

    public int Delete(Guid customerId)
    {
        var customer = context.Customers
                           .FirstOrDefault(c => c.Id == customerId)
                       ?? throw new Exception("Customer not found");

        context.Customers.Remove(customer);
        return context.SaveChanges();
    }
}
