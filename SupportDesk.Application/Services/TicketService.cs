using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;

namespace SupportDesk.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICustomerRepository _customerRepository;

    public TicketService(
        ITicketRepository ticketRepository,
        ICustomerRepository customerRepository)
    {
        _ticketRepository = ticketRepository;
        _customerRepository = customerRepository;
    }

    public async Task<List<SupportTicket>> GetAllAsync()
    {
        return await _ticketRepository.GetAllAsync();
    }

    public async Task<SupportTicket?> GetByIdAsync(Guid id)
    {
        return await _ticketRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(
        string title,
        string description,
        Guid customerId,
        TicketPriority priority)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        if (customer == null)
        {
            throw new ArgumentException("Customer was not found.");
        }

        var ticket = new SupportTicket(
            title,
            description,
            customer,
            priority);

        await _ticketRepository.AddAsync(ticket);
    }

    public async Task UpdateAsync(
        Guid id,
        string title,
        string description)
    {
        var ticket = await GetTicketOrThrowAsync(id);

        ticket.UpdateDetails(title, description);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task AssignTechnicianAsync(
        Guid id,
        string technician)
    {
        var ticket = await GetTicketOrThrowAsync(id);

        ticket.AssignTechnician(technician);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task ChangePriorityAsync(
        Guid id,
        TicketPriority priority)
    {
        var ticket = await GetTicketOrThrowAsync(id);

        ticket.ChangePriority(priority);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task ChangeStatusAsync(
        Guid id,
        TicketStatus status)
    {
        var ticket = await GetTicketOrThrowAsync(id);

        ticket.ChangeStatus(status);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task AddCommentAsync(
        Guid id,
        string text)
    {
        var ticket = await GetTicketOrThrowAsync(id);

        var comment = new Comment(text);

        ticket.AddComment(comment);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task<List<SupportTicket>> SearchAsync(
        string searchTerm)
    {
        var tickets = await _ticketRepository.GetAllAsync();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return tickets;
        }

        searchTerm = searchTerm.Trim();

        return tickets
            .Where(t =>
                t.Title.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<List<SupportTicket>> FilterByStatusAsync(
        TicketStatus status)
    {
        var tickets = await _ticketRepository.GetAllAsync();

        return tickets
            .Where(t => t.Status == status)
            .ToList();
    }

    public async Task<List<SupportTicket>> SearchAndFilterAsync(
        string searchTerm,
        TicketStatus status)
    {
        var tickets = await SearchAsync(searchTerm);

        return tickets
            .Where(t => t.Status == status)
            .ToList();
    }

    private async Task<SupportTicket> GetTicketOrThrowAsync(Guid id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        return ticket;
    }
}