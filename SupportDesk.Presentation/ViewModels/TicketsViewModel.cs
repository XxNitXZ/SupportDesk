using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    private TicketStatus? _currentStatus = null;

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
        _currentStatus = null;

        Tickets = await _ticketService.GetAllAsync();
    }

    public async Task LoadByStatusAsync(TicketStatus status)
    {
        _currentStatus = status;

        Tickets = await _ticketService.FilterByStatusAsync(status);
    }

    public async Task ApplyFilterAsync(string searchTerm, TicketStatus? status)
    {
        _currentStatus = status;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            if (status == null)
            {
                Tickets = await _ticketService.GetAllAsync();
            }
            else
            {
                Tickets = await _ticketService.FilterByStatusAsync(
                    status.Value);
            }

            return;
        }

        if (status == null)
        {
            Tickets = await _ticketService.SearchAsync(searchTerm);
        }
        else
        {
            Tickets = await _ticketService.SearchAndFilterAsync(
                searchTerm,
                status.Value);
        }
    }
}