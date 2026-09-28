using SupportDesk.Application.Interfaces;
using SupportDesk.Domain;

namespace SupportDesk.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task AddAsync(string name, string email)
    {
        var customer = new Customer(name, email);
        await _customerRepository.AddAsync(customer);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(Guid id, string name, string email)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            throw new ArgumentException("Customer was not found.");
        }

        customer.UpdateContactInfo(name, email);

        await _customerRepository.UpdateAsync(customer);
    }
}
