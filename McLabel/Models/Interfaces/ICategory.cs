using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Models.Interfaces
{
    internal interface ICategory
    {
        string Name { get; set; }
        string Color { get; set; }
        ICollection<IItem> Items { get; set; }
    }
}
