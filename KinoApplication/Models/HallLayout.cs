using System.Collections.Generic;

namespace KinoApplication.Models
{
    public class HallLayout
    {
        public int Id { get; set; }
        public List<HallRowLayout> Rows { get; set; } = new();
    }

    public class HallRowLayout
    {
        public int RowNumber { get; set; }
        public string RowLetter => ((char)('A' + RowNumber)).ToString();
        /// <summary>
        /// Jeżeli element < 0 → przerwa, w pp. numer miejsca (Column)
        /// </summary>
        public List<int> SeatColumns { get; set; } = new();
    }
}
