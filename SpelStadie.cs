using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TreIrad
{
    /// <summary>
    /// Spelmotorn som håller reda på allt om det pågående spelet.
    /// Ansvarar för att markera rutor, kolla om spelet är avslutat
    /// </summary>
    public class SpelStadie : PropertyChangeBase
    {
        private int spelPlanBredd;
        private Player vemSpelar;
        private string vinstText;
        private Visibility spelareInfoVisibility;
        private Visibility vinstInfoVisibility;

        //Konstruktorn, sätter startvärden spelet
        public SpelStadie()
        {
            spelPlanBredd = 3;
            VemSpelar = Player.X;
            SpelPasserad = 0;
            GameOver = false;
            Rutor = new ObservableCollection<Ruta>();
            SpelRutNat = new Ruta[spelPlanBredd, spelPlanBredd];
            VinstInfoVisibility = Visibility.Collapsed;
            SpelareInfoVisibility = Visibility.Visible;

            SkapaSpelRutor();
        }

        //Alla rutor som ska visas på spelbrädet, förberett för att ha olika antal rutor. 
        //En ObservableCollection kopplas till gränssnittet och talar om ifall innehåll ändras
        //Alla rutor på brädet i en array för att det är lättare att jobba med när det ska kollas om spelet är slut.
        public ObservableCollection<Ruta> Rutor { get; private set; } 
        public Ruta[,] SpelRutNat { get; private set; } 

        //Håller reda på antal markeringar
        public int SpelPasserad { get; private set; }

        public bool GameOver { get; private set; }

        public Player VemSpelar 
        {
            get { return vemSpelar; }
            private set
            {
                vemSpelar = value;
                OnPropertyChanged(nameof(VemSpelarBild));
            }
        }

        public string VemSpelarBild
        {
            get
            {
                if (VemSpelar == Player.X)
                {
                    return @"Assets\X15.png";
                }
                else
                {
                    return @"Assets\O15.png";
                }
            }
        }

        public string VinstText
        {
            get
            {
                return vinstText;
            }
            set
            {
                vinstText = value;
                OnPropertyChanged(nameof(VinstText));
            }
        }

        public Visibility VinstInfoVisibility
        {
            get
            {
                return vinstInfoVisibility;
            }
            set 
            {
                vinstInfoVisibility = value; 
                OnPropertyChanged(nameof(VinstInfoVisibility));
            }
        }

        public Visibility SpelareInfoVisibility
        {
            get
            {
                return spelareInfoVisibility;
            }
            set
            {
                spelareInfoVisibility = value;
                OnPropertyChanged(nameof(SpelareInfoVisibility));
            }
        }

        public void GorDrag(int r, int c)
        {
            if (!KanGoraSpel(r, c))
            {
                return;
            }

            SpelRutNat[r, c].VemsRuta = VemSpelar;

            SpelPasserad++;

            if (BlevAvslut(r, c, out SpelSlut? spelSlut))
            {
                GameOver = true;

                if (spelSlut?.Vinnare == Player.None)
                {
                    VinstText = "OAVGJORT";
                }
                else
                {
                    VinstText = "VINNAREN ÄR " +
                                (spelSlut?.Vinnare == Player.X ? "X" : "O"); //Vi vet att det spelslut inte är null här

                }
                VinstInfoVisibility = Visibility.Visible;
                SpelareInfoVisibility = Visibility.Collapsed;
            }
            else
            {
                VemSpelar = (VemSpelar == Player.X) ? Player.O : Player.X;
            }
        }

        public void Omstart()
        {
            SkapaSpelRutor();

            VemSpelar = Player.X;
            SpelPasserad = 0;
            GameOver = false;
            VinstInfoVisibility = Visibility.Collapsed;
            SpelareInfoVisibility = Visibility.Visible;
        }

        private void SkapaSpelRutor()
        {
            for (int rad = 0; rad < spelPlanBredd; rad++)
            {
                for (int kolumn = 0; kolumn < spelPlanBredd; kolumn++)
                {
                    var ruta = new Ruta(100, 100, kolumn, rad);
                    Rutor.Add(ruta);
                    SpelRutNat[rad, kolumn] = ruta;
                }
            }
        }

        private bool KanGoraSpel(int r, int c)
        {
            return !GameOver && SpelRutNat[r, c].VemsRuta == Player.None;

        }

        private bool ArRutNatFullt()
        {
            return SpelPasserad == 9;

        }

        private bool BlevVinst(int r, int c, out Vinstinformation? vinstinformation)
        {
            (int, int)[] row = new[] { (r, 0), (r, 1), (r, 2) };
            (int, int)[] col = new[] { (0, c), (1, c), (2, c) };
            (int, int)[] Diag = new[] { (0, 0), (1, 1), (2, 2), };
            (int, int)[] antiDiag = new[] { (0, 2), (1, 1), (2, 0), };

            if (ArRutornaMarkeradeAvSpelare(row, VemSpelar))
            {
                vinstinformation = new Vinstinformation { Type = VinstTyp.Row, Nummer = r };
                return true;
            }

            if (ArRutornaMarkeradeAvSpelare(col, VemSpelar))
            {
                vinstinformation = new Vinstinformation { Type = VinstTyp.Column, Nummer = c };
                return true;
            }

            if (ArRutornaMarkeradeAvSpelare(Diag, VemSpelar))
            {
                vinstinformation = new Vinstinformation { Type = VinstTyp.Diagonal };
                return true;

            }

            if (ArRutornaMarkeradeAvSpelare(antiDiag, VemSpelar))
            {
                vinstinformation = new Vinstinformation { Type = VinstTyp.AntiDiagonal };
                return true;
            }

            vinstinformation = null;
            return false;
        }

        private bool ArRutornaMarkeradeAvSpelare((int, int)[] squares, Player player)
        {
            foreach ((int r, int c) in squares)
            {
                if (SpelRutNat[r, c].VemsRuta != player)
                {
                    return false;
                }
            }
            return true;
        }

        private bool BlevAvslut(int r, int c, out SpelSlut? spelSlut)
        {
            if (BlevVinst(r, c, out Vinstinformation? vinstinformation))
            {
                spelSlut = new SpelSlut { Vinnare = VemSpelar, Vinstinformation = vinstinformation };
                return true;
            }

            if (ArRutNatFullt())
            {
                spelSlut = new SpelSlut { Vinnare = Player.None };
                return true;
            }

            spelSlut = null;
            return false;
        }
    }
}
