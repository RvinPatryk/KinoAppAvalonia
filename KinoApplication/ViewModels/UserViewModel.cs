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
    public class UserViewModel : ReactiveObject
    {

        public string Email => $"{Username}@example.com";

        // — Authentication state —
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set
            {
                this.RaiseAndSetIfChanged(ref _isLoggedIn, value);
                this.RaisePropertyChanged(nameof(ShowLoginPanel));
            }
        }
        public bool ShowLoginPanel => !IsLoggedIn;

        // — Current user info —
        private string _username = "Gość";
        public string Username
        {
            get => _username;
            private set
            {
                this.RaiseAndSetIfChanged(ref _username, value);
                this.RaisePropertyChanged(nameof(Email));
            }
        }

        // — Login form fields —
        private string _loginUsername = "";
        public string LoginUsername
        {
            get => _loginUsername;
            set => this.RaiseAndSetIfChanged(ref _loginUsername, value);
        }

        private string _loginPassword = "";
        public string LoginPassword
        {
            get => _loginPassword;
            set => this.RaiseAndSetIfChanged(ref _loginPassword, value);
        }

        // — Commands for auth —
        public ReactiveCommand<Unit, Unit> LoginCmd { get; }
        public ReactiveCommand<Unit, Unit> RegisterCmd { get; }
        public ReactiveCommand<Unit, Unit> LogoutCmd { get; }

        // — Repertoire & bookings —
        public ObservableCollection<Seans> Seanse { get; }
        private Seans? _wybranySeans;
        public Seans? WybranySeans
        {
            get => _wybranySeans;
            set
            {
                this.RaiseAndSetIfChanged(ref _wybranySeans, value);
                LoadSeats();
                this.RaisePropertyChanged(nameof(TotalPayment));
            }
        }

        public ObservableCollection<Seat> Seats { get; } = new();
        public ObservableCollection<RowSeatsViewModel> RowLayouts { get; } = new();
        private List<Booking> _allBookings;

        // — My reservations —
        public ObservableCollection<ReservationViewModel> Reservations { get; }

        // — Booking command & summary —
        public ReactiveCommand<Unit, Unit> ConfirmBookingCmd { get; }
        public decimal TotalPayment =>
            (WybranySeans?.Film.Price ?? 0m)
            * Seats.Count(s => s.Status == SeatStatus.Selected);

        // — Underlying accounts store —
        private List<UserAccount> _accounts;

        public UserViewModel()
        {

            // load saved accounts
            _accounts = AccountService.LoadAccounts();

            // load repertoire + bookings
            var data = RepertuarService.Load();
            Seanse = new ObservableCollection<Seans>(data.Seanse);
            _allBookings = BookingService.Load();

            // init reservations
            Reservations = new ObservableCollection<ReservationViewModel>();
            RefreshReservations();

            // login command
            LoginCmd = ReactiveCommand.Create(() =>
            {
                if (AuthService.Validate(LoginUsername, LoginPassword))
                {
                    Username = LoginUsername;
                    IsLoggedIn = true;
                    RefreshReservations();
                }
            }, this.WhenAnyValue(x => x.LoginUsername, x => x.LoginPassword,
                  (u, p) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p)));

            // register command
            RegisterCmd = ReactiveCommand.Create(() =>
            {
                AuthService.Register(LoginUsername, LoginPassword);
                Username = LoginUsername;
                IsLoggedIn = true;
                RefreshReservations();
            }, this.WhenAnyValue(x => x.LoginUsername, x => x.LoginPassword,
                  (u, p) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p)));


            // logout command
            LogoutCmd = ReactiveCommand.Create(() =>
            {
                Username = "Gość";
                IsLoggedIn = false;
                RefreshReservations();
            });

            // confirm booking
            ConfirmBookingCmd = ReactiveCommand.Create(() =>
            {
                if (WybranySeans == null || !IsLoggedIn) return;
                var key = $"{WybranySeans.Film.Id}_{WybranySeans.Sala.Id}_{WybranySeans.Data.ToUnixTimeSeconds()}";
                foreach (var seat in Seats.Where(s => s.Status == SeatStatus.Selected))
                {
                    _allBookings.Add(new Booking
                    {
                        SeansKey = key,
                        Row = seat.Row,
                        Column = seat.Column,
                        User = Username
                    });
                    seat.Status = SeatStatus.Booked;
                }
                BookingService.Save(_allBookings);
                RefreshReservations();
                this.RaisePropertyChanged(nameof(TotalPayment));
            }, this.WhenAnyValue(vm => vm.Seats.Count)
                .Select(_ => Seats.Any(s => s.Status == SeatStatus.Selected)));

        }

        private void RefreshReservations()
        {
            Reservations.Clear();
            if (!IsLoggedIn) return;
            foreach (var b in _allBookings.Where(b => b.User == Username))
                Reservations.Add(new ReservationViewModel(b, Seanse));
        }

        public void LoadSeats()
        {
            Seats.Clear();
            RowLayouts.Clear();
            if (WybranySeans == null) return;

            var hall = HallLayoutService.GetLayouts()
                        .First(h => h.Id == WybranySeans.Sala.Id);

            var key = $"{WybranySeans.Film.Id}_{WybranySeans.Sala.Id}_{WybranySeans.Data.ToUnixTimeSeconds()}";
            var booked = _allBookings
                         .Where(b => b.SeansKey == key)
                         .ToHashSet(new BookingComparer());

            foreach (var row in hall.Rows)
            {
                foreach (var col in row.SeatColumns)
                {
                    if (col < 0) continue;
                    var status = booked.Any(b => b.Row == row.RowNumber && b.Column == col)
                                 ? SeatStatus.Booked
                                 : SeatStatus.Free;
                    var seat = new Seat(row.RowNumber, col, status);
                    seat.Changed
                        .Where(x => x.PropertyName == nameof(seat.Status))
                        .Subscribe(_ => this.RaisePropertyChanged(nameof(TotalPayment)));
                    Seats.Add(seat);
                }
            }

            foreach (var row in hall.Rows)
            {
                var list = row.SeatColumns
                              .Select(col =>
                                  col < 0 ? null
                                          : Seats.First(s => s.Row == row.RowNumber && s.Column == col))
                              .ToList();
                RowLayouts.Add(new RowSeatsViewModel(row.RowLetter, list));
            }
        }

        private class BookingComparer : IEqualityComparer<Booking>
        {
            public bool Equals(Booking? x, Booking? y) =>
                x != null && y != null &&
                x.SeansKey == y.SeansKey &&
                x.Row == y.Row &&
                x.Column == y.Column;

            public int GetHashCode(Booking b) =>
                HashCode.Combine(b.SeansKey, b.Row, b.Column);
        }
    }
}
