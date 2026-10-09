using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;
using System;

namespace SupportDesk.Presentation.Views;

public sealed partial class CustomerDetailPage : Page
{
    public CustomerDetailViewModel ViewModel { get; }

    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private Guid _customerId;

    public CustomerDetailPage(
        CustomerDetailViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        _navigationService = navigationService;
        _dialogService = dialogService;

        DataContext = ViewModel;

        Loaded += CustomerDetailPage_Loaded;
    }

    public void SetCustomerId(Guid customerId)
    {
        _customerId = customerId;
    }

    private async void CustomerDetailPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await ViewModel.LoadAsync(_customerId);
        }
        catch (ArgumentException ex)
        {
            await _dialogService.ShowErrorAsync(
                "Could not load customer",
                ex.Message);
        }
    }

    private async void SaveButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await ViewModel.SaveAsync();

            await _dialogService.ShowMessageAsync(
                "Customer updated",
                "The customer was updated successfully.");

            _navigationService.GoBack();
        }
        catch (ArgumentException ex)
        {
            await _dialogService.ShowErrorAsync(
                "Could not update customer",
                ex.Message);
        }
    }

    private void CancelButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        _navigationService.GoBack();
    }
}