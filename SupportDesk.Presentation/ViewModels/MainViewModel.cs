using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportDesk.Presentation.Interfaces;

namespace SupportDesk.Presentation.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        _navigationService.NavigateToDashboard();
    }

    [RelayCommand]
    private void NavigateToTickets()
    {
        _navigationService.NavigateToTickets();
    }

    [RelayCommand]
    private void NavigateToCustomers()
    {
        _navigationService.NavigateToCustomers();
    }
}