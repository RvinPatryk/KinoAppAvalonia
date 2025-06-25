using Avalonia.Controls;
using Avalonia.Interactivity;
using KinoApplication.ViewModels;

namespace KinoApplication.Views
{
    public partial class PreviewSeatsWindow : Window
    {
        // konstruktor wywo�ywany przy przy starcie przez XAML
        public PreviewSeatsWindow()
        {
            InitializeComponent();
        }

        // konstruktor do wstrzykni�cia VM
        public PreviewSeatsWindow(AdminViewModel vm)
            : this()
        {
            DataContext = vm;
        }

        // handler przycisku Zamknij
        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}