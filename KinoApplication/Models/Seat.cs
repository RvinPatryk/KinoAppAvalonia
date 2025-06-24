using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace KinoApplication.Models
{
    public enum SeatStatus { Free, Selected, Booked }

    public class Seat : ReactiveObject
    {
        public int Row { get; }
        public int Column { get; }

        private SeatStatus _status;
        public SeatStatus Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        public ReactiveCommand<Unit, Unit> ToggleSelectCmd { get; }

        public Seat(int row, int column, SeatStatus status)
        {
            Row = row;
            Column = column;
            _status = status;

            // możesz zaznaczać tylko wolne miejsca, odznaczać tylko wybrane
            ToggleSelectCmd = ReactiveCommand.Create(() =>
            {
                if (Status == SeatStatus.Free) Status = SeatStatus.Selected;
                else if (Status == SeatStatus.Selected) Status = SeatStatus.Free;
            },
            this.WhenAnyValue(x => x.Status)
                .Select(s => s != SeatStatus.Booked));
        }
    }
}