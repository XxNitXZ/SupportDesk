using System.Text.Json;
using SupportDesk.Domain;
using SupportDesk.Domain.Tickets;

public class FullPersistenceTests
{
    [Fact]
    public async Task Ticket_ShouldKeepId_WhenSerializedAndDeserialized()
    {
        // Arrange
        var customer = new Customer(
            "Test Customer",
            "test@example.com");

        var ticket = new SupportTicket(
            "Test ticket",
            "Test description",
            customer,
            TicketPriority.High);

        var originalId = ticket.Id;
        var json = JsonSerializer.Serialize(ticket);

        // Act
        var restoredTicket =
            JsonSerializer.Deserialize<SupportTicket>(json);

        // Assert
        Assert.NotNull(restoredTicket);
        Assert.Equal(originalId, restoredTicket.Id);
        Assert.Equal(ticket.Title, restoredTicket.Title);
        Assert.Equal(ticket.Description, restoredTicket.Description);
        Assert.Equal(ticket.Priority, restoredTicket.Priority);
        Assert.Equal(ticket.Status, restoredTicket.Status);
    }

    [Fact]
    public void Ticket_ShouldKeepCreatedAt_WhenSerializedAndDeserialized()
    {
        // Arrange
        var customer = new Customer(
            "Test Customer",
            "test@example.com");

        var ticket = new SupportTicket(
            "Test ticket",
            "Test description",
            customer,
            TicketPriority.High);

        var originalCreatedAt = ticket.CreatedAt;
        var json = JsonSerializer.Serialize(ticket);

        // Act
        var restoredTicket =
            JsonSerializer.Deserialize<SupportTicket>(json);

        // Assert
        Assert.NotNull(restoredTicket);
        Assert.Equal(originalCreatedAt, restoredTicket.CreatedAt);
    }
    [Fact]
    public void Ticket_ShouldKeepCustomer_WhenSerializedAndDeserialized()
    {
        // Arrange
        var customer = new Customer(
            "Test Customer",
            "test@example.com");

        var ticket = new SupportTicket(
            "Test ticket",
            "Test description",
            customer,
            TicketPriority.High);

        var originalCustomerId = customer.Id;
        var json = JsonSerializer.Serialize(ticket);

        // Act
        var restoredTicket =
            JsonSerializer.Deserialize<SupportTicket>(json);

        // Assert
        Assert.NotNull(restoredTicket);
        Assert.Equal(originalCustomerId, restoredTicket.Customer.Id);
        Assert.Equal("Test Customer", restoredTicket.Customer.Name);
        Assert.Equal("test@example.com", restoredTicket.Customer.Email);
    }
    [Fact]
    public void Ticket_ShouldKeepTechnicianAndStatus_WhenSerializedAndDeserialized()
    {
        // Arrange
        var customer = new Customer(
            "Test Customer",
            "test@example.com");

        var ticket = new SupportTicket(
            "Test ticket",
            "Test description",
            customer,
            TicketPriority.High);

        ticket.AssignTechnician("Anna");
        ticket.ChangeStatus(TicketStatus.InProgress);

        var json = JsonSerializer.Serialize(ticket);

        // Act
        var restoredTicket =
            JsonSerializer.Deserialize<SupportTicket>(json);

        // Assert
        Assert.NotNull(restoredTicket);
        Assert.Equal("Anna", restoredTicket.Technician);
        Assert.Equal(TicketStatus.InProgress, restoredTicket.Status);
    }
}