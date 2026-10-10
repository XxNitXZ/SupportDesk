using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;
using System;

namespace SupportDesk.Presentation.Views;

public sealed partial class NewTicketPage : Page
{
    private readonly NewTicketViewModel _viewModel;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public NewTicketPage(NewTicketViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _navigationService = navigationService;
        _dialogService = dialogService;

        DataContext = _viewModel;


        Loaded += NewTicketPage_Loaded;
    }

    private async void NewTicketPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.LoadCustomersAsync();
    }

    private async void CreateTicketButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await _viewModel.CreateTicketAsync();

            await _dialogService.ShowMessageAsync(
                "Ticket created",
                "The ticket was created successfully.");

            _navigationService.NavigateToTickets();
        }
        catch (ArgumentException ex)
        {
            await _dialogService.ShowErrorAsync(
                "Could not create ticket",
                ex.Message);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                "An error occurred",
                ex.Message);
        }
    }

    private void CancelButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        _navigationService.NavigateToTickets();
    }

}