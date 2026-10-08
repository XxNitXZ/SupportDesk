using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.Interfaces;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Services;

public class DialogService : IDialogService
{
    private XamlRoot? _xamlRoot;

    public void SetXamlRoot(XamlRoot xamlRoot)
    {
        _xamlRoot = xamlRoot;
    }

    public async Task ShowMessageAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = GetXamlRoot()
        };

        await dialog.ShowAsync();
    }

    public async Task ShowErrorAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = GetXamlRoot()
        };

        await dialog.ShowAsync();
    }

    public async Task<bool> ShowConfirmAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            PrimaryButtonText = "Ja",
            CloseButtonText = "Nej",
            XamlRoot = GetXamlRoot()
        };

        var result = await dialog.ShowAsync();

        return result == ContentDialogResult.Primary;
    }

    private XamlRoot GetXamlRoot()
    {
        if (_xamlRoot == null)
        {
            throw new InvalidOperationException(
                "DialogService har ingen XamlRoot.");
        }

        return _xamlRoot;
    }
}