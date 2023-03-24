using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Models.Interfaces
{
    internal interface IItem
    {
        string Name { get; set; }
        string Category { get; set; }
        string Color { get; set; }
    }
}
