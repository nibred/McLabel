using McLabel.Commands;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace McLabel.ViewModels
{
    internal class ConfirmationWindowViewModel: NotifyBase
    {
        private string _message;
        public string Message { get => _message; set => Set(ref _message, value); }
    }
}
