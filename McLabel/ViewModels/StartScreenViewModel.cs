using McLabel.Commands;
using McLabel.Services;
using McLabel.Services.Interfaces;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xaml;

namespace McLabel.ViewModels
{
    internal class StartScreenViewModel : ViewModelBase
    {
        public ICommand OpenMainEditorCommand { get; }
        public ICommand SelectFilesCommand { get; }
        public StartScreenViewModel(ViewNavigationService viewNavigationService,
                                    IFileService xmlService,
                                    MainEditorViewModel mainEditorViewModel)
        {
            OpenMainEditorCommand = new RelayCommand(o => viewNavigationService.CurrentVM = mainEditorViewModel);
            SelectFilesCommand = new RelayCommand(o =>
            {
                if (xmlService.OpenXmlFiles())
                {
                    mainEditorViewModel.AddItems(xmlService.Items);
                    mainEditorViewModel.AddCategories(xmlService.Categories);
                    OpenMainEditorCommand.Execute(null);
                }
            });
        }
    }
}
