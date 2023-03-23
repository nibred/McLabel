using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Services
{
    internal class ViewNavigationService
    {
        private NotifyBase _currentVM;
        public NotifyBase CurrentVM
        {
            get => _currentVM; //TODO: check null reference
            set
            {
                _currentVM = value;
                CurrentViewModelChanged?.Invoke();
            }
        }
        public event Action CurrentViewModelChanged;
        public void Close() => CurrentVM = null;
    }
}
