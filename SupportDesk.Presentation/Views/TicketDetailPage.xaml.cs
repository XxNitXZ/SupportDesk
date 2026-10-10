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
    private readonly IDialogService _dialogService;

    public TicketDetailPage(
        TicketDetailViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _navigationService = navigationService;
        _dialogService = dialogService;

        DataContext = _viewModel;
    }

    public Task LoadTicketAsync(Guid ticketId)
    {
        return _viewModel.LoadAsync(ticketId);
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _viewModel.SaveAsync();

            await _dialogService.ShowMessageAsync(
                "Ticket updated",
                "The ticket was updated successfully.");

            _navigationService.GoBack();
        }
        catch (ArgumentException ex)
        {
            await _dialogService.ShowErrorAsync(
                "Could not update ticket",
                ex.Message);
        }
    }

    private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _viewModel.AddCommentAsync();
        }
        catch (ArgumentException ex)
        {
            await _dialogService.ShowErrorAsync(
                "Could not add comment",
                ex.Message);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _navigationService.GoBack();
    }
}