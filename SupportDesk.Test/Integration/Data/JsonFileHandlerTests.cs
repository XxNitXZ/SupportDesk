using SupportDesk.Infrastructure.Data;
using System.Text.Json;
using Xunit;

namespace SupportDesk.Test.Integration.Data;

public class JsonFileHandlerTests
{
    [Fact]
    public async Task ReadAsync_Should_ReturnDataFromJsonFile()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        var json = """[{"Name":"Anna"}]""";
        await File.WriteAllTextAsync(filePath, json);

        var handler = new JsonFileHandler();

        try
        {
            // Act
            var result = await handler.ReadAsync<TestCustomer>(filePath);

            // Assert
            Assert.Single(result);
            Assert.Equal("Anna", result[0].Name);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    public class TestCustomer
    {
        public string Name { get; set; } = "";
    }

    [Fact]
    public async Task ReadAsync_Should_ReturnEmptyListWhenFileDoesNotExist()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        var handler = new JsonFileHandler();

        // Act
        var result = await handler.ReadAsync<TestCustomer>(filePath);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task WriteAsync_Should_SaveDataToJsonFile()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        var handler = new JsonFileHandler();

        var customers = new List<TestCustomer>
        {
            new TestCustomer { Name = "Anna" }
        };

        try
        {
            // Act
            await handler.WriteAsync(filePath, customers);

            // Assert
            Assert.True(File.Exists(filePath));

            string json = await File.ReadAllTextAsync(filePath);

            var savedCustomers =
                JsonSerializer.Deserialize<List<TestCustomer>>(json);

            Assert.NotNull(savedCustomers);
            Assert.Single(savedCustomers);
            Assert.Equal("Anna", savedCustomers[0].Name);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public async Task ReadAsync_Should_ReturnEmptyListWhenFileIsEmpty()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        await File.WriteAllTextAsync(filePath, "");

        var handler = new JsonFileHandler();

        try
        {
            // Act
            var result = await handler.ReadAsync<TestCustomer>(filePath);

            // Assert
            Assert.Empty(result);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public async Task ReadAsync_Should_ThrowInvalidOperationExceptionWhenJsonIsInvalid()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        await File.WriteAllTextAsync(filePath, "[{");

        var handler = new JsonFileHandler();

        try
        {
            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.ReadAsync<TestCustomer>(filePath));

            // Assert
            Assert.Contains("invalid JSON", exception.Message);

            // Check that the original file content is preserved
            Assert.Equal("[{", await File.ReadAllTextAsync(filePath));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public async Task WriteAsync_Should_CreateDirectoryWhenItDoesNotExist()
    {
        // Arrange
        string directoryPath = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString());

        string filePath = Path.Combine(directoryPath, "customers.json");

        var customers = new List<TestCustomer>
        {
            new TestCustomer { Name = "Anna" }
        };

        var handler = new JsonFileHandler();

        try
        {
            // Act
            await handler.WriteAsync(filePath, customers);

            // Assert
            Assert.True(Directory.Exists(directoryPath));
            Assert.True(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, recursive: true);
            }
        }
    }
}