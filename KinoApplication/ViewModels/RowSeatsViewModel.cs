using KinoApplication.Models;
using System.Collections.Generic;

namespace KinoApplication.ViewModels
{
    public class RowSeatsViewModel
    {
        public string RowLetter { get; }
        /// <summary>
        /// Kolekcja Seat? – null oznacza przerwę, w pp. ten Seat
        /// </summary>
        public List<Seat?> SeatsInRow { get; }

        public RowSeatsViewModel(string rowLetter, List<Seat?> seatsInRow)
        {
            RowLetter = rowLetter;
            SeatsInRow = seatsInRow;
        }
    }
}
