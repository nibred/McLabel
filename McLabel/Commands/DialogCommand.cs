using McLabel.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace McLabel.Commands
{
    internal class DialogCommand : CommandBase
    {
        public bool? DialogResult { get; set; }
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var window = App.CurrentWindow;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
