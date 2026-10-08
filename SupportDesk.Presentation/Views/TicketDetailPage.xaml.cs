using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Views;

public sealed partial class TicketDetailPage : Page
{
    private readonly TicketDetailViewModel _viewModel;
    private readonly INavigationService _navigationService;

    public TicketDetailPage(
        TicketDetailViewModel viewModel,
        INavigationService navigationService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _navigationService = navigationService;

        DataContext = _viewModel;
    }

    public Task LoadTicketAsync(Guid ticketId)
    {
        return _viewModel.LoadAsync(ticketId);
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        _navigationService.GoBack();
    }
}