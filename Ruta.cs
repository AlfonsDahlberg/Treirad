using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TreIrad
{
    /// <summary>
    /// Klassen innehåller information om varje ruta på spelbrädet
    /// </summary>
    public class Ruta : PropertyChangeBase
    {
        private Player vemsRuta;

        public Ruta(int bredd, int hojd, int kolumn, int rad) 
        {
            Bredd = bredd;
            Hojd = hojd;
            Kolumn = kolumn;
            Rad = rad;
        }

        public Player VemsRuta
        {
            get
            {
                return vemsRuta;
            }
            set 
            {    
                vemsRuta = value; 
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public string ImagePath
        {
            get
            {
                switch (VemsRuta)
                {
                    case Player.O:
                        return @"Assets\O15.png";
                    case Player.X:
                        return @"Assets\X15.png";
                    case Player.None:
                    default:
                        return "";
                }
            } 
        }
        public int Bredd { get; }
        public int Hojd { get; }

        public int Kolumn { get; }
        public int Rad { get; }

        public bool KanSpelaRuta
        {
            get 
            {
                return VemsRuta == Player.None;
            }
        }
    }
}
