using Avalonia.Controls;
using Avalonia.Interactivity;
using KinoApplication.ViewModels;

namespace KinoApplication.Views
{
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
            EditFilmBtn.Click += OpenEditWindow;
        }

        private void OpenEditWindow(object? sender, RoutedEventArgs e)
        {
            var vm = (AdminViewModel)DataContext!;
            if (vm.SelectedFilm is null) return;
            var dlg = new EditFilmWindow(vm);
            dlg.ShowDialog((Window)this.VisualRoot!);
        }
    }
}
