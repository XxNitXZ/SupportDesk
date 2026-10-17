using SupportDesk.Application.Interfaces;
using SupportDesk.Domain;
using SupportDesk.Infrastructure.Data;

namespace SupportDesk.Infrastructure.Repository;

public class JsonCustomerRepository : ICustomerRepository
{
    private readonly JsonFileHandler _jsonFileHandler;
    private readonly string _filePath;

    public JsonCustomerRepository(
        JsonFileHandler jsonFileHandler,
        string? filePath = null)
    {
        _jsonFileHandler = jsonFileHandler;
        _filePath = filePath ?? DataPaths.CustomerFilePath;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _jsonFileHandler.ReadAsync<Customer>(
            _filePath);
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var customers = await GetAllAsync();

        System.Diagnostics.Debug.WriteLine(
            $"LOOKING FOR ID: {id}");

        foreach (var customer in customers)
        {
            System.Diagnostics.Debug.WriteLine(
                $"CUSTOMER FROM JSON: {customer.Name} / ID: {customer.Id}");
        }

        return customers.FirstOrDefault(customer => customer.Id == id);
    }

    public async Task AddAsync(Customer customer)
    {
        var customers = await GetAllAsync();

        customers.Add(customer);

        await _jsonFileHandler.WriteAsync(
            _filePath,
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
            _filePath,
            customers);
    }
}