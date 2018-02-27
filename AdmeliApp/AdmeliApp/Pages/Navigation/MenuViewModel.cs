using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdmeliApp.Pages.Navigation
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MenuTienda> MenuTiendaItems { get; set; }

        public MenuViewModel()
        {
            MenuTiendaItems = new ObservableCollection<MenuTienda>(new[]
            {
                new MenuTienda { Id = 0, Title = "Page 1" },
                new MenuTienda { Id = 1, Title = "Page 2" },
                new MenuTienda { Id = 2, Title = "Page 3" },
                new MenuTienda { Id = 3, Title = "Page 4" },
                new MenuTienda { Id = 4, Title = "Page 5" },
            });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
