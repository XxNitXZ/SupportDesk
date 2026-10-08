using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SupportDesk.Application.RegisterServices;
using SupportDesk.Infrastructure.RegisterServices;
using SupportDesk.Presentation.RegisterServices;
using System;

namespace SupportDesk.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    private Window? _window;

    public static ServiceProvider Services { get; private set; } = null!;

    public App()
    {
        InitializeComponent();

        var services = new ServiceCollection();

        // Register all layers
        services.AddInfrastructure();
        services.AddApplication();
        services.AddPresentation();

        Services = services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = Services.GetRequiredService<MainWindow>();
        _window.Activate();
    }
}