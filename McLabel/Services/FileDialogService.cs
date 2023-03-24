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
        private const string DEFAULT_FILENAME = "MENUDATA";
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
        public bool SaveFile(XmlDocument xmlDocument, out string selectedPath)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = FILTER,
                ValidateNames = true,
                CheckPathExists = true,
                FileName = DEFAULT_FILENAME
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                xmlDocument.Save(saveFileDialog.FileName);
                selectedPath = Path.GetDirectoryName(saveFileDialog.FileName);
                return true;
            }
            selectedPath = null;
            return false;
        }
    }
}
