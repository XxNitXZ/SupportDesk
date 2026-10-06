using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;

namespace SupportDesk.Presentation.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private readonly ITicketService _ticketService;

    private int _newCount;
    private int _inProgressCount;
    private int _resolvedCount;

    public int NewCount
    {
        get { return _newCount; }
        private set
        {
            _newCount = value;
            OnPropertyChanged();
        }
    }

    public int InProgressCount
    {
        get { return _inProgressCount; }
        private set
        {
            _inProgressCount = value;
            OnPropertyChanged();
        }
    }

    public int ResolvedCount
    {
        get { return _resolvedCount; }
        private set
        {
            _resolvedCount = value;
            OnPropertyChanged();
        }
    }

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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}