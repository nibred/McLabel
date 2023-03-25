using McLabel.Models;
using McLabel.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace McLabel.Services.Interfaces
{
    internal interface IFileService
    {
        ICollection<ICategory> Categories { get; }
        bool OpenFiles();
        bool SaveFile(ICollection<ICategory> categories);
    }
}
