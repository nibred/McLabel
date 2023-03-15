using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private string _title = "test title";
        public string Title { get => _title; set => Set(ref  _title, value); }
    }
}
