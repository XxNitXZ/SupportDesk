using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SupportDesk.Presentation.ViewModels;
using System;

namespace SupportDesk.Presentation.Views;

public sealed partial class TicketDetailPage : Page
{
    private readonly TicketDetailViewModel _viewModel;

    public TicketDetailPage(TicketDetailViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    protected override async void OnNavigatedTo(
        NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is Guid ticketId)
        {
            await _viewModel.LoadAsync(ticketId);
        }
    }
}