using McLabel.Commands;
using McLabel.Services;
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
    internal class MainViewModel : NotifyBase
    {
        private readonly ViewNavigationService _viewNavigationService;
        private readonly FileDialogService _fileDialogService;

        public NotifyBase CurrentVM => _viewNavigationService.CurrentVM;
        public ICommand CloseCommand { get; private set; }
        public MainViewModel(ViewNavigationService viewNavigationService, StartScreenViewModel startScreenViewModel, FileDialogService fileDialogService)
        {
            _viewNavigationService = viewNavigationService;
            _fileDialogService = fileDialogService;
            _viewNavigationService.CurrentVM = startScreenViewModel;
            _viewNavigationService.CurrentViewModelChanged += _viewNavigationService_CurrentViewModelChanged;
            CloseCommand = new RelayCommand(OnCloseCommandExecuted);
        }
        private void _viewNavigationService_CurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentVM));
        private void OnCloseCommandExecuted(object o)
        {
            if (_fileDialogService.ShowConfirmationDialog("Are you sure you want to exit?") != true)
                return;
            Application.Current.Shutdown();
        }
        protected override void Dispose()
        {
            _viewNavigationService.CurrentViewModelChanged -= _viewNavigationService_CurrentViewModelChanged;
            base.Dispose();
        }
    }
}
