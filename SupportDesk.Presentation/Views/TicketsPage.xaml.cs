using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Domain.Tickets;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation.Views;

public sealed partial class TicketsPage : Page
{
    private readonly TicketsViewModel _viewModel;
    private readonly INavigationService _navigationService;

    public TicketsPage(
        TicketsViewModel viewModel,
        INavigationService navigationService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _navigationService = navigationService;

        DataContext = _viewModel;

        Loaded += TicketsPage_Loaded;
    }

    private async void TicketsPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.LoadAsync();
    }

    private void NewTicketButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        _navigationService.NavigateToNewTicket();
    }

    private async void TicketsList_ItemClick(
    object sender,
    ItemClickEventArgs e)
    {
        if (e.ClickedItem is SupportTicket ticket)
        {
            System.Diagnostics.Debug.WriteLine(
                "TICKET CLICKED");

            System.Diagnostics.Debug.WriteLine(
               $"Opening ticket: {ticket.Id}");

            await _navigationService.NavigateToTicketDetailAsync(ticket.Id);
        }
    }

}