using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AdmeliApp.Pages.Navigation
{
    public class MenuGrouping : ObservableCollection<MenuTienda>
    {
        public MenuGrouping(string heading)
        {
            Heading = heading;
        }

        public string Heading { get; set; }
    }
}
