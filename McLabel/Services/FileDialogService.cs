using McLabel.ViewModels;
using McLabel.Views.Windows;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;


namespace McLabel.Services
{
    internal class FileDialogService
    {
        private const string Filter = "XML Files (*.xml)|*.xml";
        private string _selectedPath;

        public bool OpenFiles(out IEnumerable<string> selectedFiles)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "Select files",
                AddExtension = true,
                Multiselect = true,
                Filter = Filter
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
            FolderBrowserDialog saveDialog = new FolderBrowserDialog();
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                _selectedPath = saveDialog.SelectedPath;
                document.Save($"{_selectedPath}/{filename}.xml");
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
            if (confirmationVM.DontShowWindow || confirmationWindow.ShowDialog() == true)
                return true;
            return false;
        }
    }
}
