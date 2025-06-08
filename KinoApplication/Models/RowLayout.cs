using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoApplication.Models
{
    // Jeden rząd: numer oraz lista "pozycji" miejsc,
    // gdzie -1 oznacza przerwę (np. ścieżkę/aisle).
    public class RowLayout
    {
        public int RowNumber { get; set; }
        public int[] SeatLayout { get; set; } = Array.Empty<int>();
    }
}
