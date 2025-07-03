using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using KinoApplication.Models;

namespace KinoApplication.Services
{
    public static class AccountService
    {
        private const string FileName = "accounts.json";

        public static List<UserAccount> LoadAccounts()
        {
            if (!File.Exists(FileName))
                return new List<UserAccount>();
            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<List<UserAccount>>(json)
                   ?? new List<UserAccount>();
        }

        public static void SaveAccounts(List<UserAccount> accounts)
        {
            var json = JsonSerializer.Serialize(accounts,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }
    }
}
