using McLabel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Services.Interfaces
{
    internal interface IFileService
    {
        List<Item> Items { get; }
        HashSet<Category> Categories { get; }
        bool OpenXmlFiles();
    }
}
