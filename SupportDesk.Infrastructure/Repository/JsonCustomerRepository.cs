using SupportDesk.Application.Interfaces;
using SupportDesk.Domain;
using SupportDesk.Infrastructure.Data;

namespace SupportDesk.Infrastructure.Repository;

public class JsonCustomerRepository : ICustomerRepository
{
    private readonly JsonFileHandler _jsonFileHandler;

    public JsonCustomerRepository(JsonFileHandler jsonFileHandler)
    {
        _jsonFileHandler = jsonFileHandler;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _jsonFileHandler.ReadAsync<Customer>(
            DataPaths.CustomerFilePath);
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var customers = await GetAllAsync();

        return customers.FirstOrDefault(customer => customer.Id == id);
    }

    public async Task AddAsync(Customer customer)
    {
        var customers = await GetAllAsync();

        customers.Add(customer);

        await _jsonFileHandler.WriteAsync(
            DataPaths.CustomerFilePath,
            customers);
    }

    public async Task UpdateAsync(Customer customer)
    {
        var customers = await GetAllAsync();

        var index = customers.FindIndex(c => c.Id == customer.Id);

        if (index == -1)
        {
            throw new ArgumentException("Customer was not found.");
        }

        customers[index] = customer;

        await _jsonFileHandler.WriteAsync(
            DataPaths.CustomerFilePath,
            customers);
    }
}