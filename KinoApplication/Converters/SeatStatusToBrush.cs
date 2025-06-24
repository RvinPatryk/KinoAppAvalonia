using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using KinoApplication.Models;

namespace KinoApplication.Converters
{
    public class SeatStatusToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is SeatStatus status
                ? status switch
                {
                    SeatStatus.Free => Brushes.LightGreen,
                    SeatStatus.Selected => Brushes.Yellow,
                    SeatStatus.Booked => Brushes.Gray,
                    _ => Brushes.Transparent
                }
                : Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();
    }
}
