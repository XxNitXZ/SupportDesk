using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Domain;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;
using System;

namespace SupportDesk.Presentation.Views;

public sealed partial class CustomersPage : Page
{
    public CustomersViewModel ViewModel { get; }

    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public CustomersPage(
        CustomersViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        _navigationService = navigationService;
        _dialogService = dialogService;

        DataContext = ViewModel;

        Loaded += CustomersPage_Loaded;
    }

    private async void CustomersPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await ViewModel.LoadAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                "An error occurred",
                ex.Message);
        }
    }

    private void AddCustomerButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        _navigationService.NavigateToNewCustomer();
    }

    private void CustomersList_ItemClick(
        object sender,
        ItemClickEventArgs e)
    {
        if (e.ClickedItem is Customer customer)
        {
            _navigationService.NavigateToCustomerDetail(
                customer.Id);
        }
    }
}