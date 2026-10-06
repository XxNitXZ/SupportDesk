using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.ViewModels;
using Microsoft.UI.Xaml.Navigation;

namespace SupportDesk.Presentation.Views;

public sealed partial class TicketsPage : Page
{
    private readonly TicketsViewModel _viewModel;

    public TicketsPage(TicketsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    protected override async void OnNavigatedTo(
    NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        await _viewModel.LoadAsync();
    }
}