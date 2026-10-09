using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Domain;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation.Views;

public sealed partial class CustomersPage : Page
{
    public CustomersViewModel ViewModel { get; }

    private readonly INavigationService _navigationService;

    public CustomersPage(
        CustomersViewModel viewModel,
        INavigationService navigationService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        _navigationService = navigationService;

        DataContext = ViewModel;

        Loaded += CustomersPage_Loaded;
    }

    private async void CustomersPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
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