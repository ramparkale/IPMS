using IPMS.CustomerService.Entities;
using IPMS.CustomerService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IPMS.CustomerService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomersController(ICustomerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _service.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _service.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        var result = await _service.CreateAsync(customer);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.CustomerId },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        Customer customer)
    {
        var result = await _service.UpdateAsync(id, customer);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}