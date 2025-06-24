using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using KinoApplication.Models;

namespace KinoApplication.Converters
{
    public class StatusToForegroundConverter : IValueConverter
    {
        // zamienia Status → kolor napisu
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SeatStatus status && status == SeatStatus.Selected)
                return Brushes.Black;
            return Brushes.White;
        }

        // pełna sygnatura, bo IValueConverter wymaga ConvertBack
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
