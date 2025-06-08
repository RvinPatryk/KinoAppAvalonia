using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinoApplication.Models;

namespace KinoApplication.Services
{
    public static class HallLayoutService
    {
        public static List<HallLayout> GetLayouts() => new()
        {
            // Sala 1: mała, 5 rzędów po 6 miejsc
            new HallLayout {
                Id   = 1,
                Name = "Sala 1 (6×5)",
                Rows = new List<RowLayout> {
                    new() { RowNumber = 1, SeatLayout = new[]{ 1,2,3,4,5,6 } },
                    new() { RowNumber = 2, SeatLayout = new[]{ 1,2,3,4,5,6 } },
                    new() { RowNumber = 3, SeatLayout = new[]{ 1,2,3,4,5,6 } },
                    new() { RowNumber = 4, SeatLayout = new[]{ 1,2,3,4,5,6 } },
                    new() { RowNumber = 5, SeatLayout = new[]{ 1,2,3,4,5,6 } },
                }
            },

            // Sala 2: średnia, 6 rzędów, z jedną przerwą w środku (aisle)
            new HallLayout {
                Id   = 2,
                Name = "Sala 2 (8×6 z alejką)",
                Rows = new List<RowLayout> {
                    new() { RowNumber = 1, SeatLayout = new[]{ 1,2,3,-1,4,5,6,7 } },
                    new() { RowNumber = 2, SeatLayout = new[]{ 1,2,3,-1,4,5,6,7 } },
                    new() { RowNumber = 3, SeatLayout = new[]{ 1,2,3,-1,4,5,6,7 } },
                    new() { RowNumber = 4, SeatLayout = new[]{ 1,2,3,-1,4,5,6,7 } },
                    new() { RowNumber = 5, SeatLayout = new[]{ 1,2,3,-1,4,5,6,7 } },
                    new() { RowNumber = 6, SeatLayout = new[]{ 1,2,3,-1,4,5,6,7 } },
                }
            },

            // Sala 3: duża, 10 rzędów, dwie alejki (przerwy)
            new HallLayout {
                Id   = 3,
                Name = "Sala 3 (10×10 z dwiema alejkami)",
                Rows = new List<RowLayout> {
                    new() { RowNumber = 1, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 2, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 3, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 4, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 5, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 6, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 7, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 8, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber = 9, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                    new() { RowNumber =10, SeatLayout = new[]{ 1,2,3,-1,4,5,6,-1,7,8 } },
                }
            }
        };
    }
}