using ReactiveUI;
using Splat;               // RxApp
using System.Reactive;     // Unit
using KinoApplication.ViewModels;
using System;

namespace KinoApplication.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveObject _currentVm = null!;

        public ReactiveObject CurrentVm
        {
            get => _currentVm;
            set => this.RaiseAndSetIfChanged(ref _currentVm, value);
        }

        public ReactiveCommand<Unit, Unit> ShowAdminCmd { get; }
        public ReactiveCommand<Unit, Unit> ShowUserCmd { get; }

        public MainWindowViewModel()
        {
            // 1) Tworzymy komendy jako Action (blok z { }) — tak, żeby _nic_ nie zwracały
            ShowAdminCmd = ReactiveCommand.Create(
                () => { CurrentVm = new AdminViewModel(); },
                outputScheduler: RxApp.MainThreadScheduler
            );

            ShowUserCmd = ReactiveCommand.Create(
                () => { CurrentVm = new UserViewModel(); },
                outputScheduler: RxApp.MainThreadScheduler
            );

            // 2) Na początek wywołujemy komendę, by ustawić startowy VM też na wątku UI
            ShowUserCmd.Execute().Subscribe();
        }
    }
}
