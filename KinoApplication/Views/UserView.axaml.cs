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

            // zamiast Command w XAML, obs³uga w code-behind:
            WybierzSeansBtn.Click += OpenSeatsWindow;
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