using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public class TicketsViewModel
{
    private readonly ITicketService _ticketService;

    public List<SupportTicket> Tickets { get; private set; } = new();

    public TicketsViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task LoadAsync()
    {
        Tickets = await _ticketService.GetAllAsync();
        System.Diagnostics.Debug.WriteLine(
            $"Antal tickets: {Tickets.Count}");
    }

}