using SupportDesk.Application.Interfaces;
using SupportDesk.Application.Services;
using SupportDesk.Domain;
using Xunit;

namespace SupportDesk.Test.Unit.Application;

public class CustomerServiceTests
{
    [Fact]
    public async Task AddAsync_ShouldAddCustomer()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerService(repository);

        await service.AddAsync("Anna", "anna@test.se");

        var customers = await repository.GetAllAsync();

        Assert.Single(customers);
        Assert.Equal("Anna", customers[0].Name);
        Assert.Equal("anna@test.se", customers[0].Email);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCustomers()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerService(repository);

        await service.AddAsync("Anna", "anna@test.se");
        await service.AddAsync("Erik", "erik@test.se");

        var customers = await service.GetAllAsync();

        Assert.Equal(2, customers.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectCustomer()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerService(repository);

        await service.AddAsync("Anna", "anna@test.se");

        var customer = (await repository.GetAllAsync()).Single();

        var result = await service.GetByIdAsync(customer.Id);

        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
        Assert.Equal("Anna", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomer()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerService(repository);

        await service.AddAsync("Anna", "anna@test.se");

        var customer = (await repository.GetAllAsync()).Single();

        await service.UpdateAsync(
            customer.Id,
            "Anna Svensson",
            "anna.svensson@test.se");

        var updated = await service.GetByIdAsync(customer.Id);

        Assert.NotNull(updated);
        Assert.Equal("Anna Svensson", updated.Name);
        Assert.Equal("anna.svensson@test.se", updated.Email);
    }

    [Fact]
    public async Task UpdateUnknownCustomer_ShouldThrow()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerService(repository);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.UpdateAsync(
                Guid.NewGuid(),
                "Anna",
                "anna@test.se"));
    }

    private class FakeCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public Task<List<Customer>> GetAllAsync()
        {
            return Task.FromResult(_customers.ToList());
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(customer);
        }

        public Task AddAsync(Customer customer)
        {
            _customers.Add(customer);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Customer customer)
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);

            if (index >= 0)
            {
                _customers[index] = customer;
            }

            return Task.CompletedTask;
        }
    }
}