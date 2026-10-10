using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Domain.Tickets;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Views;

public sealed partial class TicketsPage : Page
{
    private readonly TicketsViewModel _viewModel;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private bool _settingFilter;

    public TicketsPage(
        TicketsViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _navigationService = navigationService;
        _dialogService = dialogService;

        DataContext = _viewModel;

        Loaded += TicketsPage_Loaded;
    }

    private async void TicketsPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        _settingFilter = true;

        BackToDashboardButton.Visibility = Visibility.Collapsed;
        SearchAndFilterGrid.Visibility = Visibility.Visible;

        StatusFilterComboBox.SelectedIndex = 0;

        try
        {
            await _viewModel.LoadAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                "An error occurred",
                ex.Message);
        }
        finally
        {
            _settingFilter = false;
        }
    }

    public async Task LoadByStatusAsync(TicketStatus status)
    {
        Loaded -= TicketsPage_Loaded;

        BackToDashboardButton.Visibility = Visibility.Visible;
        SearchAndFilterGrid.Visibility = Visibility.Collapsed;

        try
        {
            await _viewModel.LoadByStatusAsync(status);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                "An error occurred",
                ex.Message);
        }
    }

    private async void SearchTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        if (_settingFilter)
            return;

        await ApplyCurrentFilterAsync();
    }

    private async void StatusFilterComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (_settingFilter)
            return;

        await ApplyCurrentFilterAsync();
    }

    private async Task ApplyCurrentFilterAsync()
    {
        TicketStatus? status = null;

        if (StatusFilterComboBox.SelectedIndex == 1)
        {
            status = TicketStatus.New;
        }
        else if (StatusFilterComboBox.SelectedIndex == 2)
        {
            status = TicketStatus.InProgress;
        }
        else if (StatusFilterComboBox.SelectedIndex == 3)
        {
            status = TicketStatus.Resolved;
        }

        try
        {
            await _viewModel.ApplyFilterAsync(
                SearchTextBox.Text,
                status);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                "An error occurred",
                ex.Message);
        }
    }

    private void BackToDashboardButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        _navigationService.NavigateToDashboard();
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
            try
            {
                await _navigationService.NavigateToTicketDetailAsync(
                    ticket.Id);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    "An error occurred",
                    ex.Message);
            }
        }
    }
}