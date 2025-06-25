using System;
using System.Linq;
using System.Collections.Generic;
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
        // --- kolekcje danych ---
        public ObservableCollection<Film> Films { get; }
        public ObservableCollection<Sala> Sale { get; }
        public ObservableCollection<Seans> Seanse { get; }

        // --- pola dla dodawania nowego filmu ---
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

        // --- wybrany film do edycji ---
        private Film? _selectedFilm;
        public Film? SelectedFilm
        {
            get => _selectedFilm;
            set => this.RaiseAndSetIfChanged(ref _selectedFilm, value);
        }

        // --- pola dla dodawania seansu ---
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

        // --- wybrany seans do podglądu ---
        private Seans? _selectedSeans;
        public Seans? SelectedSeans
        {
            get => _selectedSeans;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSeans, value);
                LoadPreviewSeats();
                this.RaisePropertyChanged(nameof(TotalSeats));
                this.RaisePropertyChanged(nameof(BookedSeats));
                this.RaisePropertyChanged(nameof(OccupancyRate));
            }
        }

        // --- kolekcja rzędów + foteli do podglądu ---
        public ObservableCollection<RowSeatsViewModel> RowLayoutsPreview { get; }
            = new ObservableCollection<RowSeatsViewModel>();

        // --- dane analityczne ---
        public int TotalSeats => RowLayoutsPreview.Sum(r => r.SeatsInRow.Count(s => s != null));
        public int BookedSeats => RowLayoutsPreview.Sum(r => r.SeatsInRow.Count(s => s?.Status == SeatStatus.Booked));
        public double OccupancyRate => TotalSeats > 0 ? BookedSeats / (double)TotalSeats : 0.0;

        // --- komendy ---
        public ReactiveCommand<Unit, Unit> AddFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> SaveSelectedFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> DeleteFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> CancelEditCmd { get; }
        public ReactiveCommand<Unit, Unit> DodajSeansCmd { get; }

        public AdminViewModel()
        {
            // 1) Wczytaj JSON z repertuarem
            var data = RepertuarService.Load();

            // 2) Inicjalizuj kolekcje
            Films = new ObservableCollection<Film>(data.Films);
            Sale = new ObservableCollection<Sala>(data.Sale);
            Seanse = new ObservableCollection<Seans>(data.Seanse);

            // 3) Jeśli nie było sal w JSON, dodaj domyślne
            if (!Sale.Any())
            {
                Sale.Add(new Sala { Id = 1, Name = "Sala 1" });
                Sale.Add(new Sala { Id = 2, Name = "Sala 2" });
                Sale.Add(new Sala { Id = 3, Name = "Sala 3" });
            }

            // 4) Domyślna data i czas dla nowego seansu
            DataSeansuDate = DateTimeOffset.Now.Date.AddDays(5);
            DataSeansuTime = new TimeSpan(17, 0, 0);

            // 5) Dodawanie filmu
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

            // 6) Zapis edycji filmu
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

                    // failsafe: jeżeli są seanse tego filmu, nie usuwamy
                    if (Seanse.Any(s => s.Film.Id == SelectedFilm.Id))
                        return;

                    Films.Remove(SelectedFilm);
                    SaveAll();
                    SelectedFilm = null;
                },
                this.WhenAnyValue(vm => vm.SelectedFilm).Select(f => f != null)
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

            // 10) ustaw startupowy seans do podglądu
            SelectedSeans = Seanse.FirstOrDefault();
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

        private void LoadPreviewSeats()
        {
            RowLayoutsPreview.Clear();
            if (SelectedSeans == null) return;

            // layout sali
            var hall = HallLayoutService.GetLayouts()
                        .First(h => h.Id == SelectedSeans.Sala.Id);

            // klucz seansu
            var key = $"{SelectedSeans.Film.Id}_{SelectedSeans.Sala.Id}_{SelectedSeans.Data.ToUnixTimeSeconds()}";
            // wszystkie rezerwacje
            var booked = BookingService.Load()
                                .Where(b => b.SeansKey == key)
                                .ToHashSet(new BookingComparer());

            // buduj każdy wiersz
            foreach (var row in hall.Rows)
            {
                var seats = row.SeatColumns
                    .Select(col =>
                        col < 0
                          ? null
                          : new Seat(row.RowNumber, col,
                              booked.Any(b => b.Row == row.RowNumber && b.Column == col)
                                  ? SeatStatus.Booked
                                  : SeatStatus.Free))
                    .Cast<Seat?>()
                    .ToList();

                RowLayoutsPreview.Add(new RowSeatsViewModel(row.RowLetter, seats));
            }
        }

        private class BookingComparer : IEqualityComparer<Booking>
        {
            public bool Equals(Booking? x, Booking? y)
                => x != null && y != null
                   && x.SeansKey == y.SeansKey
                   && x.Row == y.Row
                   && x.Column == y.Column;
            public int GetHashCode(Booking b)
                => HashCode.Combine(b.SeansKey, b.Row, b.Column);
        }
    }
}
