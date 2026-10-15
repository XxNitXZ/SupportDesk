using SupportDesk.Application.Interfaces;
using SupportDesk.Application.Services;
using SupportDesk.Domain;
using SupportDesk.Domain.Tickets;
using Xunit;

namespace SupportDesk.Test.Unit.Application;

public class TicketServiceTests
{
    [Fact]
    public async Task Search_ShouldBeCaseInsensitive()
    {
        var (service, tickets, customers) = CreateTestSetup();

        var anna = new Customer("Anna Svensson", "anna@test.se");
        customers.Add(anna);

        tickets.Add(new SupportTicket(
            "Internet fungerar inte",
            "Problem med uppkopplingen",
            anna,
            TicketPriority.Normal));

        var result = await service.SearchAsync("INTERNET");
        var resultByCustomer = await service.SearchAsync("aNnA");

        Assert.Single(result);
        Assert.Single(resultByCustomer);
    }

    [Fact]
    public async Task FilterByStatus_ShouldReturnOnlySelected()
    {
        var (service, tickets, customers) = CreateTestSetup();

        var anna = new Customer("Anna", "anna@test.se");
        customers.Add(anna);

        var newTicket = new SupportTicket(
            "Problem med dator",
            "Datorn startar inte",
            anna,
            TicketPriority.Normal);

        var inProgressTicket = new SupportTicket(
            "Problem med nätverk",
            "Nätverket fungerar inte",
            anna,
            TicketPriority.High);

        inProgressTicket.AssignTechnician("Erik");
        inProgressTicket.ChangeStatus(TicketStatus.InProgress);

        tickets.Add(newTicket);
        tickets.Add(inProgressTicket);

        var result = await service.FilterByStatusAsync(
            TicketStatus.InProgress);

        Assert.Single(result);
        Assert.Equal(inProgressTicket.Id, result[0].Id);
        Assert.All(result, ticket =>
            Assert.Equal(TicketStatus.InProgress, ticket.Status));
    }

    [Fact]
    public async Task AddAsync_ShouldAddTicket()
    {
        var (service, tickets, customers) = CreateTestSetup();

        var anna = new Customer("Anna", "anna@test.se");
        customers.Add(anna);

        await service.AddAsync(
            "Trasig skärm",
            "Skärmen visar ingen bild",
            anna.Id,
            TicketPriority.Normal,
            null);

        Assert.Equal(1, tickets.Count);
        Assert.Equal("Trasig skärm", tickets[0].Title);
        Assert.Equal(TicketStatus.New, tickets[0].Status);
    }

    [Fact]
    public async Task AddAsync_WithUnknownCustomer_ShouldThrow()
    {
        var (service, tickets, customers) = CreateTestSetup();

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.AddAsync(
                "Trasig skärm",
                "Skärmen fungerar inte",
                Guid.NewGuid(),
                TicketPriority.Normal,
                null));

        Assert.Equal(0, tickets.Count);
    }

    [Fact]
    public async Task SearchAndFilter_ShouldApplyBoth()
    {
        var (service, tickets, customers) = CreateTestSetup();

        var anna = new Customer("Anna", "anna@test.se");
        customers.Add(anna);

        var matchingTicket = new SupportTicket(
            "Internetproblem",
            "Internet fungerar inte",
            anna,
            TicketPriority.Normal);

        matchingTicket.AssignTechnician("Erik");
        matchingTicket.ChangeStatus(TicketStatus.InProgress);

        var wrongStatus = new SupportTicket(
            "Internetproblem hemma",
            "Problem med nätverket",
            anna,
            TicketPriority.Normal);

        var wrongSearch = new SupportTicket(
            "Trasig skrivare",
            "Skrivaren fungerar inte",
            anna,
            TicketPriority.Normal);

        wrongSearch.AssignTechnician("Erik");
        wrongSearch.ChangeStatus(TicketStatus.InProgress);

        tickets.Add(matchingTicket);
        tickets.Add(wrongStatus);
        tickets.Add(wrongSearch);

        var result = await service.SearchAndFilterAsync(
            "internet",
            TicketStatus.InProgress);

        Assert.Single(result);
        Assert.Equal(matchingTicket.Id, result[0].Id);
    }

    [Fact]
    public async Task AddCommentAsync_ShouldAddComment()
    {
        var (service, tickets, customers) = CreateTestSetup();

        var anna = new Customer("Anna", "anna@test.se");
        customers.Add(anna);

        var ticket = new SupportTicket(
            "Problem med dator",
            "Datorn fungerar inte",
            anna,
            TicketPriority.Normal);

        tickets.Add(ticket);

        await service.AddCommentAsync(
            ticket.Id,
            "Vi undersöker problemet.");

        Assert.Single(ticket.Comments);
        Assert.Equal(
            "Vi undersöker problemet.",
            ticket.Comments[0].Text);
    }

    private static (
        TicketService service,
        FakeTicketRepository tickets,
        FakeCustomerRepository customers)
        CreateTestSetup()
    {
        var tickets = new FakeTicketRepository();
        var customers = new FakeCustomerRepository();

        var service = new TicketService(tickets, customers);

        return (service, tickets, customers);
    }

    private class FakeTicketRepository : ITicketRepository
    {
        private readonly List<SupportTicket> _tickets = new();

        public void Add(SupportTicket ticket)
        {
            _tickets.Add(ticket);
        }

        public Task<List<SupportTicket>> GetAllAsync()
        {
            return Task.FromResult(_tickets.ToList());
        }

        public Task<SupportTicket?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _tickets.FirstOrDefault(t => t.Id == id));
        }

        public Task AddAsync(SupportTicket ticket)
        {
            _tickets.Add(ticket);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(SupportTicket ticket)
        {
            var index = _tickets.FindIndex(t => t.Id == ticket.Id);

            if (index >= 0)
            {
                _tickets[index] = ticket;
            }

            return Task.CompletedTask;
        }

        public int Count => _tickets.Count;

        public SupportTicket this[int index] => _tickets[index];
    }

    private class FakeCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public void Add(Customer customer)
        {
            _customers.Add(customer);
        }

        public Task<List<Customer>> GetAllAsync()
        {
            return Task.FromResult(_customers.ToList());
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _customers.FirstOrDefault(c => c.Id == id));
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