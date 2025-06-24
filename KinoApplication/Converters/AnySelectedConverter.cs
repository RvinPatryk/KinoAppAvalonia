using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using KinoApplication.Models;

namespace KinoApplication.Converters
{
    public class AnySelectedConverter : IValueConverter
    {
        // value to kolekcja Seats
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IEnumerable seats)
            {
                foreach (var item in seats)
                {
                    if (item is Seat s && s.Status == SeatStatus.Selected)
                        return true;
                }
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}