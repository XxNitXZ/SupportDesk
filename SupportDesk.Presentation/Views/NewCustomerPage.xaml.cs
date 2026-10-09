using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;
using System;

namespace SupportDesk.Presentation.Views;

public sealed partial class NewCustomerPage : Page
{
    private readonly NewCustomerViewModel _viewModel;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public NewCustomerPage(
        NewCustomerViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _navigationService = navigationService;
        _dialogService = dialogService;

        DataContext = _viewModel;
    }

    private async void CreateCustomerButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("DEBUG: CreateCustomerButton_Click started");
            System.Diagnostics.Debug.WriteLine($"DEBUG: Name='{_viewModel.Name}', Email='{_viewModel.Email}'");

            await _viewModel.CreateCustomerAsync();

            System.Diagnostics.Debug.WriteLine("DEBUG: Customer created successfully");

            await _dialogService.ShowMessageAsync(
                "Customer created",
                "The customer was created successfully.");

            System.Diagnostics.Debug.WriteLine("DEBUG: Dialog shown, navigating to customers...");

            _navigationService.NavigateToCustomers();

            System.Diagnostics.Debug.WriteLine("DEBUG: Navigation completed");
        }
        catch (ArgumentException ex)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG: ArgumentException - {ex.Message}");

            await _dialogService.ShowErrorAsync(
                "Could not create customer",
                ex.Message);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG: General Exception - {ex}");

            await _dialogService.ShowErrorAsync(
                "An error occurred",
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