using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.ViewModels;
using System;

namespace SupportDesk.Presentation.Views;

public sealed partial class NewTicketPage : Page
{
    private readonly NewTicketViewModel _viewModel;

    public NewTicketPage(NewTicketViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
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
        }
        catch (ArgumentException ex)
        {
            var dialog = new ContentDialog
            {
                Title = "Could not create ticket",
                Content = ex.Message,
                CloseButtonText = "OK",
                XamlRoot = Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}