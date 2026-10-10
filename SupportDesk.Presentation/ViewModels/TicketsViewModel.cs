using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    private List<SupportTicket> _tickets = new();

    public List<SupportTicket> Tickets
    {
        get => _tickets;
        private set => SetProperty(ref _tickets, value);
    }

    public TicketsViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task LoadAsync()
    {

        Tickets = await _ticketService.GetAllAsync();

        foreach (var ticket in Tickets)
        {
            System.Diagnostics.Debug.WriteLine(
                $"LIST ID: {ticket.Id}");
        }

        System.Diagnostics.Debug.WriteLine(
            $"Antal tickets: {Tickets.Count}");
    }
}