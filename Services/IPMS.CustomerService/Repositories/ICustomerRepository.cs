using IPMS.CustomerService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPMS.CustomerService.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(int id);

    Task<Customer> AddAsync(Customer customer);

    Task UpdateAsync(Customer customer);

    Task DeleteAsync(Customer customer);
}