using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Services
{
    internal class FileDialogService
    {
        public bool OpenFiles(out IEnumerable<string> selectedFiles)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Title = "Select files",
                AddExtension = true,
                Multiselect = true,
                Filter = "XML Files (*.xml)|*.xml"
            };
            if (openDialog.ShowDialog() == true)
            {
                selectedFiles = openDialog.FileNames;
                return true;
            }
            selectedFiles = null;
            return false;
        }
    }
}
