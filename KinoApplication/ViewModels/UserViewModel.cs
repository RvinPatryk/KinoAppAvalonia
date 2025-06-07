using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoApplication.ViewModels
{
    public class UserViewModel : ReactiveObject
    {
        private string _welcome = "Panel użytkownika – wybierz seans";

        public string Welcome
        {
            get => _welcome;
            set => this.RaiseAndSetIfChanged(ref _welcome, value);
        }
    }
}
