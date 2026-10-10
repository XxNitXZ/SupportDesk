using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        var tickets = await _ticketRepository.GetAllAsync();

        return tickets
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToList();
    }

    public async Task<SupportTicket?> GetByIdAsync(Guid id)
    {
        return await _ticketRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(
        string title,
        string description,
        Guid customerId,
        TicketPriority priority,
        string? technician)
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

        if (!string.IsNullOrWhiteSpace(technician))
        {
            ticket.AssignTechnician(technician);
        }

        await _ticketRepository.AddAsync(ticket);
    }

    public async Task UpdateTicketAsync(
        Guid id,
        string title,
        string description,
        TicketPriority priority,
        string? technician,
        TicketStatus status)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        ticket.UpdateDetails(title, description);

        ticket.ChangePriority(priority);

        if (!string.IsNullOrWhiteSpace(technician))
        {
            ticket.AssignTechnician(technician);
        }

        ticket.ChangeStatus(status);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task UpdateAsync(
        Guid id,
        string title,
        string description)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        ticket.UpdateDetails(
            title,
            description);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task AssignTechnicianAsync(
        Guid id,
        string technician)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        ticket.AssignTechnician(technician);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task ChangePriorityAsync(
        Guid id,
        TicketPriority priority)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        ticket.ChangePriority(priority);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task ChangeStatusAsync(
        Guid id,
        TicketStatus status)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        ticket.ChangeStatus(status);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task AddCommentAsync(
        Guid id,
        string text)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }

        var comment = new Comment(text);

        ticket.AddComment(comment);

        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task<List<SupportTicket>> SearchAsync(
        string searchTerm)
    {
        var tickets = await GetAllAsync();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return tickets;
        }

        searchTerm = searchTerm.Trim();

        return tickets
            .Where(ticket =>
                ticket.Title.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)
                ||
                ticket.Customer.Name.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<List<SupportTicket>> FilterByStatusAsync(
        TicketStatus status)
    {
        var tickets = await GetAllAsync();

        return tickets
            .Where(ticket => ticket.Status == status)
            .ToList();
    }

    public async Task<List<SupportTicket>> SearchAndFilterAsync(
        string searchTerm,
        TicketStatus status)
    {
        var tickets = await FilterByStatusAsync(status);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return tickets;
        }

        searchTerm = searchTerm.Trim();

        return tickets
            .Where(ticket =>
                ticket.Title.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)
                ||
                ticket.Customer.Name.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}