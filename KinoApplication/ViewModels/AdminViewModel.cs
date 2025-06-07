using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;

namespace KinoApplication.ViewModels
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
    }

    public class Sala
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class Seans
    {
        public Film Film { get; set; } = null!;
        public Sala Sala { get; set; } = null!;
        public DateTimeOffset Data { get; set; }
    }

    public class AdminViewModel : ReactiveObject
    {
        public ObservableCollection<Film> Films { get; }
            = new ObservableCollection<Film>
            {
                new() { Id = 1, Title = "Matrix" },
                new() { Id = 2, Title = "Incepcja" },
                new() { Id = 3, Title = "Interstellar" }
            };

        public ObservableCollection<Sala> Sale { get; }
            = new ObservableCollection<Sala>
            {
                new() { Id = 1, Name = "Sala 1" },
                new() { Id = 2, Name = "Sala 2" }
            };

        public ObservableCollection<Seans> Seanse { get; }
            = new ObservableCollection<Seans>();

        private Film? _wybranyFilm;
        public Film? WybranyFilm
        {
            get => _wybranyFilm;
            set => this.RaiseAndSetIfChanged(ref _wybranyFilm, value);
        }

        private Sala? _wybranaSala;
        public Sala? WybranaSala
        {
            get => _wybranaSala;
            set => this.RaiseAndSetIfChanged(ref _wybranaSala, value);
        }

        private DateTimeOffset _dataSeansu = DateTimeOffset.Now;
        public DateTimeOffset DataSeansu
        {
            get => _dataSeansu;
            set => this.RaiseAndSetIfChanged(ref _dataSeansu, value);
        }

        public ReactiveCommand<Unit, Unit> DodajSeansCmd { get; }

        public AdminViewModel()
        {
            // komenda dodająca nowy seans
            DodajSeansCmd = ReactiveCommand.Create(() =>
            {
                if (WybranyFilm is null || WybranaSala is null)
                    return;

                Seanse.Add(new Seans
                {
                    Film = WybranyFilm,
                    Sala = WybranaSala,
                    Data = DataSeansu
                });
            });

            // kilka przykładowych seansów na start
            var jutro = DateTimeOffset.Now.Date.AddDays(1);
            Seanse.Add(new Seans
            {
                Film = Films[0],
                Sala = Sale[0],
                Data = jutro.AddHours(18)
            });
            Seanse.Add(new Seans
            {
                Film = Films[1],
                Sala = Sale[1],
                Data = jutro.AddHours(20).AddMinutes(30)
            });
        }
    }
}
