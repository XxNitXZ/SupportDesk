using SupportDesk.Domain;
using SupportDesk.Infrastructure.Data;
using SupportDesk.Infrastructure.Repository;
using Xunit;

namespace SupportDesk.Test.Integration.Data;

public class JsonCustomerRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldSaveAndLoadCustomer()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        var customer = new Customer(
            "Anna",
            "anna@example.com");

        var repository = new JsonCustomerRepository(
            new JsonFileHandler(),
            filePath);

        try
        {
            // Act
            await repository.AddAsync(customer);

            var newRepository = new JsonCustomerRepository(
                new JsonFileHandler(),
                filePath);

            var savedCustomer = await newRepository.GetByIdAsync(customer.Id);

            // Assert
            Assert.NotNull(savedCustomer);
            Assert.Equal(customer.Id, savedCustomer.Id);
            Assert.Equal(customer.Name, savedCustomer.Name);
            Assert.Equal(customer.Email, savedCustomer.Email);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}