using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using KinoApplication.Models;

namespace KinoApplication.Services
{
    public static class BookingService
    {
        private static readonly string FilePath = "bookings.json";
        private static readonly JsonSerializerOptions Opts = new() { WriteIndented = true };

        public static List<Booking> Load()
        {
            if (!File.Exists(FilePath))
                return new List<Booking>();
            try
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Booking>>(json, Opts)
                       ?? new List<Booking>();
            }
            catch
            {
                return new List<Booking>();
            }
        }

        public static void Save(List<Booking> bookings)
        {
            var json = JsonSerializer.Serialize(bookings, Opts);
            File.WriteAllText(FilePath, json);
        }
    }
}