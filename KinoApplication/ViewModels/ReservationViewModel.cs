using System;
using System.Linq;
using System.Collections.ObjectModel;
using ReactiveUI;
using KinoApplication.Models;
using KinoApplication.Services;
using KinoApplication.ViewModels;

namespace KinoApplication.ViewModels
{
    public class ReservationViewModel : ReactiveObject
    {
        public string FilmTitle { get; }
        public string SalaName { get; }
        public DateTimeOffset Data { get; }
        public int Row { get; }
        public int Column { get; }

        public ReservationViewModel(Booking booking, ObservableCollection<Seans> seanse)
        {
            // znajdź odpowiadający seans przez klucz
            var seans = seanse.FirstOrDefault(s =>
                $"{s.Film.Id}_{s.Sala.Id}_{s.Data.ToUnixTimeSeconds()}" == booking.SeansKey
            );

            FilmTitle = seans?.Film.Title ?? string.Empty;
            SalaName = seans?.Sala.Name ?? string.Empty;
            Data = seans?.Data ?? DateTimeOffset.MinValue;
            Row = booking.Row;
            Column = booking.Column;
        }
    }
}
