using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreIrad
{
    /// <summary>
    /// Information om vilken rad eller kolumn som blev vinsten
    /// </summary>
    public class Vinstinformation 
    {
        public VinstTyp Type { get; set; }
        public int Nummer { get; set; }
    }
}
