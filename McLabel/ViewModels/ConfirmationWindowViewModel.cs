using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.ViewModels
{
    internal class ConfirmationWindowViewModel : NotifyBase
    {
        private string _message = "Are you sure you want to delete?";
        public string Message { get => _message; set => Set(ref _message, value); }
    }
}
