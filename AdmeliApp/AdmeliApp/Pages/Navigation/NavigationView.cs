using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Pages.Navigation
{
    public class NavigationView : ContentView
    {
        public void OnNavigationItemSelected(NavigationItemSelectedEventArgs e)
        {
            NavigationItemSelected?.Invoke(this, e);
        }

        public event NavigationItemSelectedEventHandler NavigationItemSelected;
    }

    /// <summary>
    /// Argumentos para pasar a los controladores de eventos
    /// </summary>
    public class NavigationItemSelectedEventArgs : EventArgs
    {
        public int Index { get; set; }
    }

    public delegate void NavigationItemSelectedEventHandler(object sender, NavigationItemSelectedEventArgs e);
}
