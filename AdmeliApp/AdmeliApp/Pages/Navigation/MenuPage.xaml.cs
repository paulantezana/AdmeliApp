using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.Navigation
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        public ListView ListView;

        public MenuPage ()
		{
			InitializeComponent ();
            BindingContext = new MenuViewModel();

            ListView = MenuItemsListView;
        }
	}
}