using Avalonia.Controls;
using Avalonia.Interactivity;
using KinoApplication.ViewModels;
using System;

namespace KinoApplication.Views
{
    public partial class SelectSeatsWindow : Window
    {
        public SelectSeatsWindow() => InitializeComponent();

        public SelectSeatsWindow(UserViewModel vm) : this()
        {
            DataContext = vm;
            vm.LoadSeats();

            ConfirmBtn.Click += (_, __) =>
            {
                vm.ConfirmBookingCmd.Execute().Subscribe();
                Close();
            };
            CancelBtn.Click += (_, __) =>
            {
                vm.LoadSeats(); // odrzuć zaznaczenia
                Close();
            };
        }
    }
}