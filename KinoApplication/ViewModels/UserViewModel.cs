using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;

namespace KinoApplication.ViewModels
{
    public class UserViewModel : ReactiveObject
    {
        // --- SAMPLE DATA ---
        public ObservableCollection<Seans> Seanse { get; }
            = new ObservableCollection<Seans>
            {
                new Seans {
                    Film = new Film { Id = 1, Title = "Matrix" },
                    Sala = new Sala { Id = 1, Name = "Sala 1" },
                    Data = DateTimeOffset.Now.Date.AddDays(1).AddHours(18)
                },
                new Seans {
                    Film = new Film { Id = 2, Title = "Incepcja" },
                    Sala = new Sala { Id = 2, Name = "Sala 2" },
                    Data = DateTimeOffset.Now.Date.AddDays(1).AddHours(20).AddMinutes(30)
                },
                new Seans {
                    Film = new Film { Id = 3, Title = "Interstellar" },
                    Sala = new Sala { Id = 1, Name = "Sala 1" },
                    Data = DateTimeOffset.Now.Date.AddDays(2).AddHours(19)
                }
            };

        private Seans? _wybranySeans;
        public Seans? WybranySeans
        {
            get => _wybranySeans;
            set => this.RaiseAndSetIfChanged(ref _wybranySeans, value);
        }

        public ReactiveCommand<Unit, Unit> WybierzSeansCmd { get; }

        public UserViewModel()
        {
            WybierzSeansCmd = ReactiveCommand.Create(() =>
            {
                if (WybranySeans is null) return;
                // tutaj później logika rezerwacji
            });
        }
    }
}
