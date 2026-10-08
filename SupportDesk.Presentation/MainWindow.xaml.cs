using Microsoft.UI.Xaml;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation;

public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }

    private readonly IDialogService _dialogService;

    public MainWindow(
        MainViewModel viewModel,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        _dialogService = dialogService;

        RootGrid.DataContext = ViewModel;

        navigationService.SetFrame(MainFrame);

        RootGrid.Loaded += RootGrid_Loaded;

        navigationService.NavigateToDashboard();
    }

    private void RootGrid_Loaded(object sender, RoutedEventArgs e)
    {
        _dialogService.SetXamlRoot(RootGrid.XamlRoot);
    }
}