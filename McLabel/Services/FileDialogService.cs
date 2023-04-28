using McLabel.ViewModels;
using McLabel.Views.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace McLabel.Services
{
    internal class FileDialogService
    {
        private const string _FILTER = "XML Files (*.xml)|*.xml";
        private string _selectedPath;

        public bool OpenFiles(out IEnumerable<string> selectedFiles)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Title = "Select files",
                AddExtension = true,
                Multiselect = true,
                Filter = _FILTER
            };
            if (openDialog.ShowDialog() == true)
            {
                selectedFiles = openDialog.FileNames;
                return true;
            }
            selectedFiles = null;
            return false;
        }
        public bool SaveFile(XmlDocument document, string filename)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = _FILTER,
                ValidateNames = true,
                CheckPathExists = true,
                FileName = filename
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _selectedPath = Path.GetDirectoryName(saveFileDialog.FileName);
                document.Save(saveFileDialog.FileName);
                return true;
            }
            return false;
        }
        public bool SaveFileWithoutDialog(XmlDocument document, string filename)
        {
            try
            {
                document.Save($"{_selectedPath}/{filename}.xml");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ShowConfirmationDialog(string message)
        {
            var confirmationWindow = new ConfirmationWindow();
            var confirmationVM = (ConfirmationWindowViewModel)confirmationWindow.DataContext;
            confirmationVM.Message = message;
            if (confirmationWindow.ShowDialog() == true)
                return true;
            return false;
        }
    }
}
