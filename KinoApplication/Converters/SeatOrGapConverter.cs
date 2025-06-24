using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media;
using KinoApplication.Models;

namespace KinoApplication.Converters
{
    public class SeatOrGapConverter : IValueConverter
    {
        public object? Convert(object? value, Type _, object? __, CultureInfo ___)
        {
            if (value is Seat seat)
            {
                // gap przed miejscem 3 (column==2) i przed miejscem 9 (column==8)
                var leftExtra =
                    (seat.Column == 2 || seat.Column == 8)
                        ? 20
                        : 0;

                var btn = new Button
                {
                    Content = (seat.Column + 1).ToString(),
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(2 + leftExtra, 2, 2, 2),
                    DataContext = seat
                };
                btn.Command = seat.ToggleSelectCmd;

                // tło wg statusu
                btn.Bind(Button.BackgroundProperty,
                         new Avalonia.Data.Binding("Status")
                         {
                             Source = seat,
                             Converter = new SeatStatusToBrushConverter()
                         });

                // tekst czarny gdy Selected
                btn.Bind(Button.ForegroundProperty,
                         new Avalonia.Data.Binding("Status")
                         {
                             Source = seat,
                             Converter = new StatusToForegroundConverter()
                         });

                return btn;
            }

            // null-wartość (gap) -> nic nie rysujemy, bo marginesami sterujemy przerwą
            return null;
        }

        public object? ConvertBack(object? value, Type ObjectType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
