using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SupportDesk.Presentation.ViewModels;

namespace SupportDesk.Presentation.Views
{
    public sealed partial class CustomersPage : Page
    {
        public CustomersViewModel ViewModel { get; }
        public CustomersPage(CustomersViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += CustomersPage_Loaded;
        }
        private async void CustomersPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }
    }
}
