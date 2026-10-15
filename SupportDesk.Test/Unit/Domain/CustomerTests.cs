using SupportDesk.Domain;

namespace SupportDesk.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Constructor_Should_CreateCustomerWhenValuesAreValid()
    {
        // Arrange
        var name = "Hans Mattin-Lassei";
        var email = "hans@example.com";

        // Act
        var customer = new Customer(name, email);

        // Assert
        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
    }

    [Fact]
    public void Constructor_Should_GenerateUniqueIdsForDifferentCustomers()
    {
        // Arrange & Act
        var firstCustomer = new Customer("Hans", "hans@example.com");
        var secondCustomer = new Customer("Anna", "anna@example.com");

        // Assert
        Assert.NotEqual(firstCustomer.Id, secondCustomer.Id);
    }

    [Fact]
    public void Constructor_Should_ThrowExceptionWhenNameContainsMultipleSpaces()
    {
        // Arrange
        var name = "Hans   Mattin";

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(name, "hans@example.com"));

        // Assert
        Assert.Equal("Name cannot contain multiple spaces.", exception.Message);
    }

    [Fact]
    public void Constructor_Should_ThrowExceptionWhenEmailContainsSpaces()
    {
        // Arrange
        var email = "hans mattin@example.com";

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer("Hans", email));

        // Assert
        Assert.Equal("Email cannot contain spaces.", exception.Message);
    }

    [Fact]
    public void Constructor_Should_ThrowExceptionWhenEmailDoesNotContainAtSymbol()
    {
        // Arrange
        var email = "hans.example.com";

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer("Hans", email));

        // Assert
        Assert.Equal("Email is not valid.", exception.Message);
    }

    [Theory]
    [InlineData("  Hans Mattin  ", " hans@example.com ", "Hans Mattin", "hans@example.com")]
    [InlineData("\tAnna Andersson\n", "\tanna@example.com ", "Anna Andersson", "anna@example.com")]
    public void Constructor_Should_TrimWhitespaceFromNameAndEmail(
        string name,
        string email,
        string expectedName,
        string expectedEmail)
    {
        // Act
        var customer = new Customer(name, email);

        // Assert
        Assert.Equal(expectedName, customer.Name);
        Assert.Equal(expectedEmail, customer.Email);
    }

    [Fact]
    public void UpdateContactInfo_Should_UpdateNameAndEmailWhenValuesAreValid()
    {
        // Arrange
        var customer = new Customer("Hans", "hans@example.com");

        // Act
        customer.UpdateContactInfo("Anna Andersson", "anna@example.com");

        // Assert
        Assert.Equal("Anna Andersson", customer.Name);
        Assert.Equal("anna@example.com", customer.Email);
    }

    [Fact]
    public void UpdateContactInfo_Should_TrimWhitespaceFromNameAndEmail()
    {
        // Arrange
        var customer = new Customer("Hans", "hans@example.com");

        // Act
        customer.UpdateContactInfo("  Anna Andersson  ", " anna@example.com ");

        // Assert
        Assert.Equal("Anna Andersson", customer.Name);
        Assert.Equal("anna@example.com", customer.Email);
    }

    [Fact]
    public void UpdateContactInfo_Should_ThrowExceptionWhenNameIsEmpty()
    {
        // Arrange
        var customer = new Customer("Hans", "hans@example.com");

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            customer.UpdateContactInfo("", "anna@example.com"));

        // Assert
        Assert.Equal("Name cannot be empty.", exception.Message);
    }

    [Fact]
    public void UpdateContactInfo_Should_ThrowExceptionWhenEmailIsInvalid()
    {
        // Arrange
        var customer = new Customer("Hans", "hans@example.com");

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            customer.UpdateContactInfo("Anna", "invalid-email"));

        // Assert
        Assert.Equal("Email is not valid.", exception.Message);
    }
}