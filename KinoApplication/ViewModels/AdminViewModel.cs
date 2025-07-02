using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using KinoApplication.Models;
using KinoApplication.Services;
using System.Collections.Generic;

namespace KinoApplication.ViewModels
{
    public class AdminViewModel : ReactiveObject
    {

        // Logowanie
        private string _username = "";
        public string Username { get => _username; set => this.RaiseAndSetIfChanged(ref _username, value); }

        private string _password = "";
        public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

        private bool _isAuthenticated = false;
        public bool IsAuthenticated { get => _isAuthenticated; set => this.RaiseAndSetIfChanged(ref _isAuthenticated, value); }

        public ReactiveCommand<Unit, Unit> LoginCmd { get; }

        // Kolekcje repertuaru
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

        // Wybrany seans do podglądu/edycji
        private Seans? _selectedSeans;
        public Seans? SelectedSeans
        {
            get => _selectedSeans;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSeans, value);
                if (value != null)
                {
                    EditSeansDate = value.Data.Date;
                    EditSeansTime = value.Data.TimeOfDay;
                }
                LoadPreviewSeats();
            }
        }

        // Pola w oknie edycji seansu
        private DateTimeOffset _editSeansDate;
        public DateTimeOffset EditSeansDate
        {
            get => _editSeansDate;
            set => this.RaiseAndSetIfChanged(ref _editSeansDate, value);
        }

        private TimeSpan _editSeansTime;
        public TimeSpan EditSeansTime
        {
            get => _editSeansTime;
            set => this.RaiseAndSetIfChanged(ref _editSeansTime, value);
        }

        // Kolekcja rzędów + foteli do podglądu
        public ObservableCollection<RowSeatsViewModel> RowLayoutsPreview { get; }
            = new ObservableCollection<RowSeatsViewModel>();

        // Komendy
        public ReactiveCommand<Unit, Unit> AddFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> SaveSelectedFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> DeleteFilmCmd { get; }
        public ReactiveCommand<Unit, Unit> CancelEditCmd { get; }
        public ReactiveCommand<Unit, Unit> DodajSeansCmd { get; }
        public ReactiveCommand<Unit, Unit> SaveSeansCmd { get; }

        public AdminViewModel()
        {
            var data = RepertuarService.Load();
            Films = new ObservableCollection<Film>(data.Films);
            Sale = new ObservableCollection<Sala>(data.Sale);
            Seanse = new ObservableCollection<Seans>(data.Seanse);

            if (!Sale.Any())
            {
                Sale.Add(new Sala { Id = 1, Name = "Sala 1" });
                Sale.Add(new Sala { Id = 2, Name = "Sala 2" });
                Sale.Add(new Sala { Id = 3, Name = "Sala 3" });
            }

            DataSeansuDate = DateTimeOffset.Now.Date.AddDays(5);
            DataSeansuTime = TimeSpan.FromHours(17);

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

                NewFilmTitle = NewFilmPrice = NewFilmDescription = "";
                SaveAll();
            });

            SaveSelectedFilmCmd = ReactiveCommand.Create(
                () =>
                {
                    SaveAll();
                    SelectedFilm = null;
                },
                this.WhenAnyValue(vm => vm.SelectedFilm).Select(f => f != null)
            );

            DeleteFilmCmd = ReactiveCommand.Create(
                () =>
                {
                    if (SelectedFilm is null) return;
                    if (Seanse.Any(s => s.Film.Id == SelectedFilm.Id)) return;
                    Films.Remove(SelectedFilm);
                    SaveAll();
                    SelectedFilm = null;
                },
                this.WhenAnyValue(vm => vm.SelectedFilm).Select(f => f != null)
            );

            CancelEditCmd = ReactiveCommand.Create(() => { SelectedFilm = null; });

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

            SaveSeansCmd = ReactiveCommand.Create(
                () =>
                {
                    if (SelectedSeans == null) return;
                    SelectedSeans.Data = EditSeansDate.Add(EditSeansTime);
                    SaveAll();
                    LoadPreviewSeats();
                },
                this.WhenAnyValue(vm => vm.SelectedSeans).Select(s => s != null)
            );

            SelectedSeans = Seanse.FirstOrDefault();

            LoginCmd = ReactiveCommand.Create(() =>
            {
                if (Username == "admin" && Password == "admin")
                    IsAuthenticated = true;
            },
            this.WhenAnyValue(x => x.Username, x => x.Password,
                              (u, p) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p)));
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

            var hall = HallLayoutService
                        .GetLayouts()
                        .First(h => h.Id == SelectedSeans.Sala.Id);

            var key = $"{SelectedSeans.Film.Id}_{SelectedSeans.Sala.Id}_{SelectedSeans.Data.ToUnixTimeSeconds()}";
            var booked = BookingService.Load()
                         .Where(b => b.SeansKey == key)
                         .ToHashSet(new BookingComparer());

            foreach (var row in hall.Rows)
            {
                var list = row.SeatColumns
                              .Select(col => col < 0
                                  ? (Seat?)null
                                  : new Seat(row.RowNumber, col,
                                      booked.Any(b => b.Row == row.RowNumber && b.Column == col)
                                      ? SeatStatus.Booked
                                      : SeatStatus.Free))
                              .ToList();

                RowLayoutsPreview.Add(new RowSeatsViewModel(row.RowLetter, list));
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
