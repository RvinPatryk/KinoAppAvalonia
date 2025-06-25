using Avalonia.Controls;
using Avalonia.Interactivity;
using KinoApplication.ViewModels;

namespace KinoApplication.Views
{
    public partial class PreviewSeatsWindow : Window
    {
        // konstruktor wywo³ywany przy przy starcie przez XAML
        public PreviewSeatsWindow()
        {
            InitializeComponent();
        }

        // konstruktor do wstrzykniêcia VM
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