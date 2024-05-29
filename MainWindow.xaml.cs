using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreIrad
{
    /// <summary>
    /// Huvudföntret med spelplan som visas när spelet startas.
    /// Innehåller huvudklassen spelstadie som är spelmotorn
    /// Och Event för knappklick skickas in i spelmotorn
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpelStadie spelStadie;

        public MainWindow()
        {
            InitializeComponent();

            spelStadie = new SpelStadie();
            this.DataContext = spelStadie;
        }

        private void Ruta_Click(object sender, RoutedEventArgs e)
        {
            var klickadKnapp = sender as Button;
            var klickadRuta = klickadKnapp?.DataContext as Ruta;
            if (klickadRuta == null)
            {
                return;
            }
            spelStadie.GorDrag(klickadRuta.Rad, klickadRuta.Kolumn);
        }

        private void NyttSpel_Click(object sender, RoutedEventArgs e)
        {
            spelStadie.Omstart();
        }
    }
}
