using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using SupportDesk.Domain.Tickets;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation.Views;

public sealed partial class TicketsPage : Page
{
    private readonly TicketsViewModel _viewModel;

    public TicketsPage(TicketsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    protected override async void OnNavigatedTo(
    NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        await _viewModel.LoadAsync();
    }
    private void Ticket_Tapped(
    object sender,
    TappedRoutedEventArgs e)
    {
        if (sender is FrameworkElement element &&
            element.DataContext is SupportTicket ticket)
        {
            Frame.Navigate(
                typeof(TicketDetailPage),
                ticket.Id);
        }
    }
}