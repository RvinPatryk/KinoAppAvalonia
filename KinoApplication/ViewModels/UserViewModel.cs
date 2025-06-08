using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using KinoApplication.Models;
using KinoApplication.Services;

namespace KinoApplication.ViewModels
{
    public class UserViewModel : ReactiveObject
    {
        public ObservableCollection<Seans> Seanse { get; }
        private Seans? _wybranySeans;
        public Seans? WybranySeans
        {
            get => _wybranySeans;
            set => this.RaiseAndSetIfChanged(ref _wybranySeans, value);
        }

        public ReactiveCommand<Unit, Unit> WybierzSeansCmd { get; }

        public UserViewModel()
        {
            // Wczytaj repertuar z JSON
            var data = RepertuarService.Load();
            Seanse = new ObservableCollection<Seans>(data.Seanse);

            WybierzSeansCmd = ReactiveCommand.Create(() =>
            {
                // logika rezerwacji miejsc
            });
        }
    }
}
