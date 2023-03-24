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
        private const string MAIN_FILE = "MENUDATA";
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
        public bool SaveFile(XmlDocument document)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = FILTER,
                ValidateNames = true,
                CheckPathExists = true,
                FileName = MAIN_FILE
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                document.Save(saveFileDialog.FileName);
                return true;
            }
            return false;
        }
    }
}
