using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using KinoApplication.Models;
using KinoApplication.ViewModels;
using System.Linq;
using System;

namespace KinoApplication.Views
{
    public partial class EditFilmWindow : Window
    {
        // zapobiegamy CS8618 przez inicjalizację „null!” – zostanie ustawione w drugim ctorze
        private readonly AdminViewModel _vm = null!;
        private readonly Film _original = null!;
        private readonly Film _tempCopy = null!;

        public EditFilmWindow()
        {
            InitializeComponent();
        }

        public EditFilmWindow(AdminViewModel vm) : this()
        {
            _vm = vm;
            _original = vm.SelectedFilm!;

            // tworzę kopię
            _tempCopy = new Film
            {
                Id = _original.Id,
                Title = _original.Title,
                Price = _original.Price,
                Description = _original.Description
            };

            DataContext = _tempCopy;

            SaveBtn.Click += (_, __) =>
            {
                // przenosimy zmiany z kopii do oryginału
                _original.Title = _tempCopy.Title;
                _original.Price = _tempCopy.Price;
                _original.Description = _tempCopy.Description;

                _vm.SaveSelectedFilmCmd.Execute().Subscribe();
                Close();
            };

            DeleteBtn.Click += async (_, __) =>
            {
                bool hasSeanse = _vm.Seanse.Any(s => s.Film.Id == _original.Id);
                if (hasSeanse)
                {
                    var alert = new Window
                    {
                        Title = "Błąd",
                        Width = 300,
                        Height = 120,
                        Content = new TextBlock
                        {
                            Text = "Nie można usunąć filmu, są do niego przypisane seanse.",
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(10)
                        }
                    };
                    await alert.ShowDialog((Window)this.VisualRoot!);
                    return;
                }

                _vm.DeleteFilmCmd.Execute().Subscribe();
                Close();
            };

            CancelBtn.Click += (_, __) => Close();
        }
    }
}