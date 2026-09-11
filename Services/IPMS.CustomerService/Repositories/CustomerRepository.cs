using IPMS.CustomerService.Data;
using IPMS.CustomerService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPMS.CustomerService.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerId == id);
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);

        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Customer customer)
    {
        customer.IsActive = false;

        _context.Customers.Update(customer);

        await _context.SaveChangesAsync();
    }
}