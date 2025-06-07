using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace KinoApplication.ViewModels
{
    public class AdminViewModel : ReactiveObject
    {
        private string _info = "Panel administratora";

        public string Info
        {
            get => _info;
            set => this.RaiseAndSetIfChanged(ref _info, value);
        }
    }
}
