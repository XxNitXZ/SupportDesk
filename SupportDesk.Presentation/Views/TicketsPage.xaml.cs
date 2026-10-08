using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
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

    //private async void Ticket_Tapped(
    //    object sender,
    //    TappedRoutedEventArgs e)
    //{
    //    if (sender is FrameworkElement element &&
    //        element.DataContext is SupportTicket ticket)
    //    {
    //        await _navigationService.NavigateToTicketDetailAsync(ticket.Id);
    //    }
    //}

    private void OpenTicketDetails_Click(object sender, RoutedEventArgs e)
    {
        _navigationService.NavigateToTicketDetail();
    }

}