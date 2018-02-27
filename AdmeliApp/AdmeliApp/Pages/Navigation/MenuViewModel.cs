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
        public ObservableCollection<MenuGrouping> MenuTiendaItems { get; set; }

        public MenuViewModel()
        {
            MenuTiendaItems = new ObservableCollection<MenuGrouping>()
            {
                new MenuGrouping("Grupo 1")
                {
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 1" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 2" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 3" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 4" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 5" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 6" }
                },

                new MenuGrouping("Grupo 2")
                {
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 1" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 2" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 3" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 4" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 5" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Menu 6" }
                }
            };
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
