using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System.Linq;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    [ObservableProperty]
    public partial int NewCount { get; set; }

    [ObservableProperty]
    public partial int InProgressCount { get; set; }

    [ObservableProperty]
    public partial int ResolvedCount { get; set; }

    public DashboardViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task LoadAsync()
    {
        var tickets = await _ticketService.GetAllAsync();

        NewCount = tickets.Count(ticket =>
            ticket.Status == TicketStatus.New);

        InProgressCount = tickets.Count(ticket =>
            ticket.Status == TicketStatus.InProgress);

        ResolvedCount = tickets.Count(ticket =>
            ticket.Status == TicketStatus.Resolved);
    }
}