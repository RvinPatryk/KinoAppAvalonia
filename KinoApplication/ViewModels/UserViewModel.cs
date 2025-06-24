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
        public ObservableCollection<Seans> Seanse { get; }
        private Seans? _wybranySeans;
        public Seans? WybranySeans
        {
            get => _wybranySeans;
            set => this.RaiseAndSetIfChanged(ref _wybranySeans, value);
        }

        public ObservableCollection<Seat> Seats { get; } = new();
        public ObservableCollection<RowSeatsViewModel> RowLayouts { get; } = new();

        private List<Booking> _allBookings;

        public ReactiveCommand<Unit, Unit> WybierzSeansCmd { get; }
        public ReactiveCommand<Unit, Unit> ConfirmBookingCmd { get; }

        public UserViewModel()
        {
            var data = RepertuarService.Load();
            Seanse = new ObservableCollection<Seans>(data.Seanse);
            _allBookings = BookingService.Load();

            WybierzSeansCmd = ReactiveCommand.Create(LoadSeats,
                this.WhenAnyValue(vm => vm.WybranySeans).Select(s => s != null));

            ConfirmBookingCmd = ReactiveCommand.Create(() =>
            {
                if (WybranySeans == null) return;
                var key = $"{WybranySeans.Film.Id}_{WybranySeans.Sala.Id}_{WybranySeans.Data.ToUnixTimeSeconds()}";
                _allBookings.RemoveAll(b => b.SeansKey == key);
                foreach (var seat in Seats.Where(s => s.Status == SeatStatus.Selected))
                {
                    _allBookings.Add(new Booking { SeansKey = key, Row = seat.Row, Column = seat.Column });
                    seat.Status = SeatStatus.Booked;
                }
                BookingService.Save(_allBookings);
            },
            this.WhenAnyValue(vm => vm.RowLayouts.Count)
                .Select(_ => RowLayouts.Any(
                   rl => rl.SeatsInRow.Any(s => s?.Status == SeatStatus.Selected)))
            );
        }

        public void LoadSeats()
        {
            Seats.Clear();
            RowLayouts.Clear();
            if (WybranySeans == null) return;

            // 1) layout sali
            var hall = HallLayoutService.GetLayouts()
                        .First(h => h.Id == WybranySeans.Sala.Id);

            // 2) rezerwacje
            var key = $"{WybranySeans.Film.Id}_{WybranySeans.Sala.Id}_{WybranySeans.Data.ToUnixTimeSeconds()}";
            var booked = _allBookings
                         .Where(b => b.SeansKey == key)
                         .ToHashSet(new BookingComparer());

            // 3) lista Seat
            foreach (var row in hall.Rows)
            {
                foreach (var col in row.SeatColumns)
                {
                    if (col < 0) continue;
                    var status = booked.Any(b => b.Row == row.RowNumber && b.Column == col)
                                 ? SeatStatus.Booked
                                 : SeatStatus.Free;
                    Seats.Add(new Seat(row.RowNumber, col, status));
                }
            }

            // 4) budowanie RowLayouts
            foreach (var row in hall.Rows)
            {
                var list = row.SeatColumns.Select(col =>
                    col < 0
                        ? null
                        : Seats.First(s => s.Row == row.RowNumber && s.Column == col)
                ).ToList();
                RowLayouts.Add(new RowSeatsViewModel(row.RowLetter, list));
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
