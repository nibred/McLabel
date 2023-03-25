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
        private const string FILTER = "XML Files (*.xml)|*.xml";
        private string _selectedPath;
        public bool OpenFiles(out IEnumerable<string> selectedFiles)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Title = "Select files",
                AddExtension = true,
                Multiselect = true,
                Filter = FILTER
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
                Filter = FILTER,
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
    }
}
