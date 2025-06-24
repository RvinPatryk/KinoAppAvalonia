// Services/HallLayoutService.cs
using System.Collections.Generic;
using System.Linq;
using KinoApplication.Models;

namespace KinoApplication.Services
{
    public static class HallLayoutService
    {
        public static List<HallLayout> GetLayouts()
        {
            return new List<HallLayout>
            {
                // SALA 1 – rozbudowana, 7 rzędów A–G
                new HallLayout {
                    Id = 1,
                    Rows = new List<HallRowLayout>
                    {
                        // A  
                        new HallRowLayout {
                            RowNumber   = 0,
                            SeatColumns= new List<int> {
                                0,1,    // miejsca 1–2
                               -1,-1,   // przerwa 2 kolumny
                                2,3,4,5,6,7  // miejsca 3–8
                            }
                        },
                        // B (9 miejsc z przesunięciem 9-tego)
                        new HallRowLayout {
                            RowNumber   = 1,
                            SeatColumns= new List<int> {
                                0,1, -1,-1, 2,3,4,5,6,7,  // miejsca 1–8
                               -1,    // przerwa przed miejscem 9
                                8      // miejsce 9
                            }
                        },
                        // C
                        new HallRowLayout {
                            RowNumber   = 2,
                            SeatColumns= new List<int> {
                                0,1, -1,-1, 2,3,4,5,6,7, -1, 8
                            }
                        },
                        // D
                        new HallRowLayout {
                            RowNumber   = 3,
                            SeatColumns= new List<int> {
                                0,1, -1,-1, 2,3,4,5,6,7, -1, 8
                            }
                        },
                        // E
                        new HallRowLayout {
                            RowNumber   = 4,
                            SeatColumns= new List<int> {
                                0,1, -1,-1, 2,3,4,5,6,7, -1, 8
                            }
                        },
                        // F (8 miejsc)
                        new HallRowLayout {
                            RowNumber   = 5,
                            SeatColumns= new List<int> {
                                0,1, -1,-1, 2,3,4,5,6,7
                            }
                        },
                        // G (8 miejsc)
                        new HallRowLayout {
                            RowNumber   = 6,
                            SeatColumns= new List<int> {
                                0,1, -1,-1, 2,3,4,5,6,7
                            }
                        },
                    }
                },

                // SALA 2 – mniejsza, 5 rzędów po 6 miejsc
                new HallLayout {
                    Id = 2,
                    Rows = new List<HallRowLayout>
                    {
                        new HallRowLayout { RowNumber=0, SeatColumns = new[]{0,1,-1,-1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=1, SeatColumns = new[]{0,1,-1,-1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=2, SeatColumns = new[]{0,1,-1,-1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=3, SeatColumns = new[]{0,1,-1,-1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=4, SeatColumns = new[]{0,1,-1,-1,2,3}.ToList() },
                    }
                },

                // SALA 3 – VIP, 4 rzędy po 4 miejsca, bez przerw
                new HallLayout {
                    Id = 3,
                    Rows = new List<HallRowLayout>
                    {
                        new HallRowLayout { RowNumber=0, SeatColumns = new[]{0,1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=1, SeatColumns = new[]{0,1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=2, SeatColumns = new[]{0,1,2,3}.ToList() },
                        new HallRowLayout { RowNumber=3, SeatColumns = new[]{0,1,2,3}.ToList() },
                    }
                }
            };
        }
    }
}
