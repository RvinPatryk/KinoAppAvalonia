using System;
using System.IO;
using System.Text.Json;
using KinoApplication.Models;

namespace KinoApplication.Services
{
    public class RepertuarData
    {
        public Film[] Films { get; set; } = Array.Empty<Film>();
        public Sala[] Sale { get; set; } = Array.Empty<Sala>();
        public Seans[] Seanse { get; set; } = Array.Empty<Seans>();
    }

    public static class RepertuarService
    {
        private static readonly string FilePath = "repertuar.json";
        private static readonly JsonSerializerOptions Opts = new()
        {
            WriteIndented = true
        };

        // Wczytaj lub utwórz nowy plik
        public static RepertuarData Load()
        {
            if (!File.Exists(FilePath))
            {
                var empty = new RepertuarData();
                Save(empty);
                return empty;
            }

            var json = File.ReadAllText(FilePath);
            try
            {
                return JsonSerializer.Deserialize<RepertuarData>(json, Opts)
                    ?? new RepertuarData();
            }
            catch
            {
                // uszkodzony JSON? nadpisz domyślnym
                var empty = new RepertuarData();
                Save(empty);
                return empty;
            }
        }

        public static void Save(RepertuarData data)
        {
            var json = JsonSerializer.Serialize(data, Opts);
            File.WriteAllText(FilePath, json);
        }
    }
}
