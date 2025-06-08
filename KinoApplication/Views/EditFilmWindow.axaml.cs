using Avalonia.Controls;
using KinoApplication.Models;
using KinoApplication.ViewModels;
using System;

namespace KinoApplication.Views
{
    public partial class EditFilmWindow : Window
    {
        private readonly Film _original;
        private readonly Film _tempCopy;
        private readonly AdminViewModel _vm;

        public EditFilmWindow() => InitializeComponent();

        public EditFilmWindow(AdminViewModel vm) : this()
        {
            _vm = vm;
            _original = vm.SelectedFilm!;   // zachowaj referencję do oryginału

            // zrób płytką kopię (bez ObservableCollection, tylko pola)
            _tempCopy = new Film
            {
                Id = _original.Id,
                Title = _original.Title,
                Price = _original.Price,
                Description = _original.Description
            };

            // na kopii będzie działać Twoje x:DataType bindings
            DataContext = _tempCopy;

            SaveBtn.Click += (_, __) =>
            {
                // przenieś zmiany z kopii do oryginału
                _original.Title = _tempCopy.Title;
                _original.Price = _tempCopy.Price;
                _original.Description = _tempCopy.Description;

                // zapisz JSON i zamknij
                _vm.SaveSelectedFilmCmd.Execute().Subscribe();
                Close();
            };

            DeleteBtn.Click += (_, __) =>
            {
                _vm.DeleteFilmCmd.Execute().Subscribe();
                Close();
            };

            CancelBtn.Click += (_, __) =>
            {
                // nie dotykaj oryginału – po prostu zamknij okno
                Close();
            };
        }
    }
}
