using Avalonia.Controls;
using Avalonia.Interactivity;
using KinoApplication.ViewModels;

namespace KinoApplication.Views
{
    public partial class PreviewSeatsWindow : Window
    {
        // Parametryzowany i domyœlny konstruktor
        public PreviewSeatsWindow() => InitializeComponent();
        public PreviewSeatsWindow(AdminViewModel vm) : this()
        {
            DataContext = vm;
        }

        // Handler przycisku Anuluj
        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}