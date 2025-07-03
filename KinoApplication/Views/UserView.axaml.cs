using Avalonia.Controls;
using Avalonia.Interactivity;
using KinoApplication.ViewModels;
using KinoApplication.Views;

namespace KinoApplication.Views
{
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();

            ShowReservationsBtn.Click += OnShowReservations;

            ShowAccountBtn.Click += OnShowAccount;
        }

        private void OnShowReservations(object? sender, RoutedEventArgs e)
        {
            ReservationsPanel.IsVisible = !ReservationsPanel.IsVisible;
            if (ReservationsPanel.IsVisible)
                AccountPanel.IsVisible = false;
        }

        private void OnShowAccount(object? sender, RoutedEventArgs e)
        {
            AccountPanel.IsVisible = !AccountPanel.IsVisible;
            if (AccountPanel.IsVisible)
                ReservationsPanel.IsVisible = false;
        }

        private void OpenSeatsWindow(object? sender, RoutedEventArgs e)
        {
            var vm = (UserViewModel)DataContext!;
            if (vm.WybranySeans is null) return;

            var dlg = new SelectSeatsWindow(vm);
            dlg.ShowDialog((Window)this.VisualRoot!);
        }
    }
}