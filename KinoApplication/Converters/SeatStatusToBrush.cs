using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using KinoApplication.Models;
using System.Globalization;

namespace KinoApplication.Converters
{
    public class SeatStatusToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is SeatStatus status
                ? status switch
                {
                    SeatStatus.Free => Brushes.LightGreen,
                    SeatStatus.Selected => Brushes.Yellow,
                    SeatStatus.Booked => Brushes.Red,      // zmiana na czerwony
                    _ => Brushes.Transparent
                }
                : Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}