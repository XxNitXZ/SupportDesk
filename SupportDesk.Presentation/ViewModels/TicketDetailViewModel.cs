using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public class TicketDetailViewModel
{
    private readonly ITicketService _ticketService;

    public TicketDetailViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public SupportTicket? Ticket { get; private set; }

    public async Task LoadAsync(Guid ticketId)
    {
        Ticket = await _ticketService.GetByIdAsync(ticketId);

        if (Ticket == null)
        {
            throw new ArgumentException("Ticket was not found.");
        }
    }
}