using System;
using System.Linq;
using KinoApplication.Models;

namespace KinoApplication.Services
{
    public static class AuthService
    {

        public static bool Validate(string username, string password)
        {
            var accounts = AccountService.LoadAccounts();
            var acc = accounts
                .FirstOrDefault(a =>
                    a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (acc == null)
                return false;

            // teraz porównujemy wprost
            return acc.PasswordHash == password;
        }


        public static bool Register(string username, string password)
        {
            var accounts = AccountService.LoadAccounts();
            if (accounts.Any(a =>
                a.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            var acc = new UserAccount
            {
                Username = username,
                PasswordHash = password,               
                Email = $"{username}@example.com"
            };
            accounts.Add(acc);
            AccountService.SaveAccounts(accounts);
            return true;
        }
    }
}
