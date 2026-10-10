using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Domain.Tickets;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel { get; }

    private readonly INavigationService _navigationService;

    public DashboardPage(
        DashboardViewModel viewModel,
        INavigationService navigationService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        _navigationService = navigationService;

        DataContext = ViewModel;

        Loaded += DashboardPage_Loaded;
    }

    private async void DashboardPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    private async void NewTicketsButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await _navigationService.NavigateToTicketsByStatusAsync(
            TicketStatus.New);
    }

    private async void InProgressTicketsButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await _navigationService.NavigateToTicketsByStatusAsync(
            TicketStatus.InProgress);
    }

    private async void ResolvedTicketsButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await _navigationService.NavigateToTicketsByStatusAsync(
            TicketStatus.Resolved);
    }
}