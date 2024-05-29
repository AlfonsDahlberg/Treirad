using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreIrad
{
    /// <summary>
    /// En basklass för att testa arv.
    /// Den innehåller koden för att tala om för gränssnittet att en egenskap i spelmotorn eller i en ruta ändrats.
    /// </summary>
    public class PropertyChangeBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
