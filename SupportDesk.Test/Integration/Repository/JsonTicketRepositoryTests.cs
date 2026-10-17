using SupportDesk.Domain;
using SupportDesk.Domain.Tickets;
using SupportDesk.Infrastructure.Data;
using SupportDesk.Infrastructure.Repository;
using Xunit;

namespace SupportDesk.Test.Integration.Repository;

public class JsonTicketRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldSaveAndLoadTicketWithCustomerAndComment()
    {
        // Arrange
        string directoryPath = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString());

        string customerFilePath = Path.Combine(
            directoryPath, "customers.json");

        string ticketFilePath = Path.Combine(
            directoryPath, "tickets.json");

        var fileHandler = new JsonFileHandler();

        var customerRepository = new JsonCustomerRepository(
            fileHandler, customerFilePath);

        var ticketRepository = new JsonTicketRepository(
            fileHandler, ticketFilePath);

        var customer = new Customer(
            "Anna",
            "anna@example.com");

        var ticket = new SupportTicket(
            "Internet problem",
            "Internet is not working",
            customer,
            TicketPriority.High);

        ticket.AssignTechnician("Erik");
        ticket.ChangeStatus(TicketStatus.InProgress);
        ticket.AddComment(new Comment("Working on the problem"));

        try
        {
            // Act - Save customer and ticket
            await customerRepository.AddAsync(customer);
            await ticketRepository.AddAsync(ticket);

            // Create new repository instances
            var newCustomerRepository = new JsonCustomerRepository(
                new JsonFileHandler(), customerFilePath);

            var newTicketRepository = new JsonTicketRepository(
                new JsonFileHandler(), ticketFilePath);

            // Load saved data
            var savedCustomer = await newCustomerRepository.GetByIdAsync(
                customer.Id);

            var savedTicket = await newTicketRepository.GetByIdAsync(
                ticket.Id);

            // Assert
            Assert.NotNull(savedCustomer);
            Assert.NotNull(savedTicket);

            Assert.Equal(customer.Id, savedCustomer.Id);
            Assert.Equal(customer.Name, savedCustomer.Name);
            Assert.Equal(customer.Email, savedCustomer.Email);

            Assert.Equal(ticket.Id, savedTicket.Id);
            Assert.Equal(ticket.Title, savedTicket.Title);
            Assert.Equal(ticket.Description, savedTicket.Description);
            Assert.Equal(ticket.Priority, savedTicket.Priority);

            Assert.Equal(customer.Id, savedTicket.Customer.Id);

            Assert.Equal("Erik", savedTicket.Technician);
            Assert.Equal(TicketStatus.InProgress, savedTicket.Status);

            Assert.Single(savedTicket.Comments);
            Assert.Equal(
                "Working on the problem",
                savedTicket.Comments[0].Text);
        }
        finally
        {
            // Delete temporary test files
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, recursive: true);
            }
        }
    }
}