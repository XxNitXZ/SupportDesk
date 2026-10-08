using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel { get; }

    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        DataContext = ViewModel;

        Loaded += DashboardPage_Loaded;
    }

    private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}