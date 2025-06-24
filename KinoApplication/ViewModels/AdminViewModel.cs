using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using KinoApplication.Models;
using KinoApplication.Services;

namespace KinoApplication.ViewModels
{
    public class AdminViewModel : ReactiveObject
    {
        // Kolekcje
        public ObservableCollection<Film> Films { get; }
        public ObservableCollection<Sala> Sale { get; }
        public ObservableCollection<Seans> Seanse { get; }

        // Pola do dodawania filmu
        private string _newFilmTitle = "";
        public string NewFilmTitle
        {
            get => _newFilmTitle;
            set => this.RaiseAndSetIfChanged(ref _newFilmTitle, value);
        }

        private string _newFilmPrice = "";
        public string NewFilmPrice
        {
            get => _newFilmPrice;
            set => this.RaiseAndSetIfChanged(ref _newFilmPrice, value);
        }

        private string _newFilmDescription = "";
        public string NewFilmDescription
        {
            get => _newFilmDescription;
            set => this.RaiseAndSetIfChanged(ref _newFilmDescription, value);
        }

        // Wybrany w liście film (do edycji)
        private Film? _selectedFilm;
        public Film? SelectedFilm
        {
            get => _selectedFilm;
            set => this.RaiseAndSetIfChanged(ref _selectedFilm, value);
        }

        // Pola do dodawania seansu
        public Film? NewWybranyFilm { get; set; }
        public Sala? NewWybranaSala { get; set; }
        private DateTimeOffset? _dataSeansuDate;
        public DateTimeOffset? DataSeansuDate
        {
            get => _dataSeansuDate;
            set => this.RaiseAndSetIfChanged(ref _dataSeansuDate, value);
        }
        private TimeSpan _dataSeansuTime;
        public TimeSpan DataSeansuTime
        {
            get => _dataSeansuTime;
            set => this.RaiseAndSetIfChanged(ref _dataSeansuTime, value);
        }

        // Komendy
        public ReactiveCommand<Unit, Unit> AddFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> SaveSelectedFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> DeleteFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> CancelEditCmd { get; }
        public ReactiveCommand<Unit, Unit> DodajSeansCmd { get; }

        public AdminViewModel()
        {
            // 1) Wczytaj lub utwórz JSON
            var data = RepertuarService.Load();

            // 2) Inicjalizacja kolekcji
            Films = new ObservableCollection<Film>(data.Films);
            Sale = new ObservableCollection<Sala>(data.Sale);
            Seanse = new ObservableCollection<Seans>(data.Seanse);

            // 3) Domyślne sale jeśli JSON ich nie zawierał
            if (!Sale.Any())
            {
                Sale.Add(new Sala { Id = 1, Name = "Sala 1" });
                Sale.Add(new Sala { Id = 2, Name = "Sala 2" });
                Sale.Add(new Sala { Id = 3, Name = "Sala 3" });
            }

            // 4) Domyślna data i czas dla seansu
            DataSeansuDate = DateTimeOffset.Now.Date.AddDays(5);
            DataSeansuTime = new TimeSpan(17, 0, 0);

            // 5) Dodawanie nowego filmu
            AddFilmCmd = ReactiveCommand.Create(() =>
            {
                if (string.IsNullOrWhiteSpace(NewFilmTitle) ||
                    !decimal.TryParse(NewFilmPrice, out var price))
                    return;

                var nextId = Films.Any() ? Films.Max(f => f.Id) + 1 : 1;
                Films.Add(new Film
                {
                    Id = nextId,
                    Title = NewFilmTitle.Trim(),
                    Price = price,
                    Description = NewFilmDescription.Trim()
                });

                NewFilmTitle = "";
                NewFilmPrice = "";
                NewFilmDescription = "";

                SaveAll();
            });

            // 6) Zapis edytowanego filmu
            SaveSelectedFilmCmd = ReactiveCommand.Create(
                () =>
                {
                    SaveAll();
                    SelectedFilm = null;
                },
                this.WhenAnyValue(vm => vm.SelectedFilm).Select(f => f != null)
            );



            // 7) Usuwanie filmu
            DeleteFilmCmd = ReactiveCommand.Create(
                () =>
                {
                    if (SelectedFilm is null)
                        return;

                    // failsafe: jeżeli są seanse tego filmu, pomiń usuwanie
                    var hasSeanse = Seanse.Any(s => s.Film.Id == SelectedFilm.Id);
                    if (hasSeanse)
                        return;

                    Films.Remove(SelectedFilm);
                    SaveAll();
                    SelectedFilm = null;
                },

                // komenda dostępna tylko gdy wybrano jakiś film
                this.WhenAnyValue(vm => vm.SelectedFilm)
                    .Select(f => f != null)
            );

            // 8) Anulowanie edycji
            CancelEditCmd = ReactiveCommand.Create(() =>
            {
                SelectedFilm = null;
            });

            // 9) Dodawanie seansu
            DodajSeansCmd = ReactiveCommand.Create(() =>
            {
                if (NewWybranyFilm is null ||
                    NewWybranaSala is null ||
                    DataSeansuDate is null)
                    return;

                var dto = DataSeansuDate.Value.Add(DataSeansuTime);
                Seanse.Add(new Seans
                {
                    Film = NewWybranyFilm,
                    Sala = NewWybranaSala,
                    Data = dto
                });

                SaveAll();
            });
        }

        private void SaveAll()
        {
            RepertuarService.Save(new RepertuarData
            {
                Films = Films.ToArray(),
                Sale = Sale.ToArray(),
                Seanse = Seanse.ToArray()
            });
        }
    }
}
