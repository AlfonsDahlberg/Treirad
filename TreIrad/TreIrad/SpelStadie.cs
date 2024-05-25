using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreIrad
{
    public class SpelStadie
    {
        public Player[,] SpelRutNat { get; private set; }
        public Player VemSpelar { get; private set; }
        public int SpelPasserad { get; private set; }
        public bool GameOver { get; private set; }

        public event Action<int, int> SpelGjort;
        public event Action<SpelSlut> SpelAvslutat;
        public event Action SpelOmstart;

        public SpelStadie()
        {
            SpelRutNat = new Player[3, 3];
            VemSpelar = Player.X;
            SpelPasserad = 0;
            GameOver = false;

        }

        private bool KanGoraSpel(int r, int c)
        {
            return !GameOver && SpelRutNat[r, c] == Player.None;

        }

        private bool ArRutNatFullt()
        {
            return SpelPasserad == 9;

        }

        private void BytPlayer()
        {
            if (VemSpelar == Player.X)
            {
                VemSpelar = Player.O;
            }
            else
            {
                VemSpelar = Player.X;
            }
        }
            private bool ArRutaMarkerad((int, int)[] squares, Player player)
            {
                foreach ((int r, int c) in squares)
                {
                    if (SpelRutNat[r, c] != player)
                    {
                        return false;
                    }
                }
                return true;
            }

            private bool BlevVinst(int r, int c, out Vinstinformation vinstinformation)
            {
                (int, int)[] row = new[] { (r, 0), (r, 1), (r, 2) };
                (int, int)[] col = new[] { (0, c), (1, c), (2, c) };
                (int, int)[] Diag = new[] { (0, 0), (1, 1), (2, 2), };
                (int, int)[] antiDiag = new[] { (0, 2), (1, 1), (2, 0), };

                if (ArRutaMarkerad(row, VemSpelar))
                {
                    vinstinformation = new Vinstinformation { Type = VinstTyp.Row, Nummer = r };
                    return true;
                }

                if (ArRutaMarkerad(col, VemSpelar))
                {
                    vinstinformation = new Vinstinformation { Type = VinstTyp.Column, Nummer = c };
                    return true;
                }

                if (ArRutaMarkerad(Diag, VemSpelar))
                {
                    vinstinformation = new Vinstinformation { Type = VinstTyp.Diagonal };
                    return true;
                
                }

                if (ArRutaMarkerad(antiDiag, VemSpelar))
                {
                    vinstinformation = new Vinstinformation { Type=VinstTyp.AntiDiagonal };
                    return true;
                }

                vinstinformation = null;
                return false;
            }

            private bool BlevAvslut(int r, int c, out SpelSlut spelSlut)
            { 
                if (BlevVinst(r, c ,out Vinstinformation vinstinformation))
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

            public void GorDrag(int r, int c)
            { 
                if (!KanGoraSpel(r, c))
                { 
                    return; 
                }
                SpelRutNat[r, c] = VemSpelar;
                SpelPasserad++;

                if (BlevAvslut(r, c, out SpelSlut spelSlut))
                { 
                    GameOver = true;
                    SpelGjort?.Invoke(r, c);
                    SpelAvslutat?.Invoke(spelSlut);
                }
                else 
                {
                    BytPlayer();
                    SpelGjort?.Invoke(r, c);
                }
                    
                
            }
        public void Omstart() 
        {
            SpelRutNat = new Player[3, 3];
            VemSpelar = Player.X;
            SpelPasserad = 0;
            GameOver = false;
            SpelOmstart?.Invoke();
        }
    }
}
