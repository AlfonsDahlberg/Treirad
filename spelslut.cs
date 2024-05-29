using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreIrad
{
    /// <summary>
    /// Klass för spelets slut och vinnare.
    /// </summary>
    public class SpelSlut 
    {
        public Player Vinnare { get; set; } 
        public Vinstinformation? Vinstinformation { get; set; }
    }
}
