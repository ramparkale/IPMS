using IPMS.CustomerService.Entities;
using IPMS.CustomerService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPMS.CustomerService.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        customer.CreatedDate = DateTime.UtcNow;
        customer.IsActive = true;

        return await _repository.AddAsync(customer);
    }

    public async Task<bool> UpdateAsync(int id, Customer customer)
    {
        var existing = await _repository.GetByIdAsync(id);

        if (existing == null)
            return false;

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;
        existing.Phone = customer.Phone;
        existing.Address = customer.Address;
        existing.City = customer.City;
        existing.State = customer.State;
        existing.Pincode = customer.Pincode;
        existing.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(existing);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);

        if (customer == null)
            return false;

        await _repository.DeleteAsync(customer);

        return true;
    }
}