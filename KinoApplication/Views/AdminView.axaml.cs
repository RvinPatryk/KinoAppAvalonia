using System;
using Avalonia.Interactivity;
using ReactiveUI;
using System.Reactive.Disposables;
using KinoApplication.ViewModels;
using Avalonia.ReactiveUI;

namespace KinoApplication.Views
{
    public partial class AdminView : ReactiveUserControl<AdminViewModel>
    {
        public AdminView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // obs³uga przycisku "Edytuj film"
                EditFilmBtn.Click += OnEditFilm;
                Disposable
                    .Create(() => EditFilmBtn.Click -= OnEditFilm)
                    .DisposeWith(disposables);

                // obs³uga przycisku "Podgl¹d i edycja seansu"
                PreviewSeansBtn.Click += OnPreviewSeans;
                Disposable
                    .Create(() => PreviewSeansBtn.Click -= OnPreviewSeans)
                    .DisposeWith(disposables);
            });
        }

        private void OnEditFilm(object? sender, RoutedEventArgs e)
        {
            if (ViewModel?.SelectedFilm is null)
                return;

            var dlg = new EditFilmWindow(ViewModel);
            dlg.Show();
        }

        private void OnPreviewSeans(object? sender, RoutedEventArgs e)
        {
            if (ViewModel?.SelectedSeans is null)
                return;

            var dlg = new PreviewSeatsWindow(ViewModel);
            dlg.Show();
        }
    }
}
