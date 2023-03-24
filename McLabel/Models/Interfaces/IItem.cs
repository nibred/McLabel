using Microsoft.SqlServer.Server;
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
        string Exp1Days { get; set; }
        string Exp1Hours { get; set; }
        string Exp1Minutes { get; set; }
        string Exp1Message { get; set; }
        string Exp2Days { get; set; }
        string Exp2Hours { get; set; }
        string Exp2Minutes { get; set; }
        string Exp2Message { get; set; }
        string Format { get; set; }
        string Line1st { get; set; }
        string Line2nd { get; set; }
        string Color { get; set; }
    }
}
