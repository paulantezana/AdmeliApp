using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.CompraPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProveedorPage : ContentPage
	{
		public ProveedorPage ()
		{
			InitializeComponent ();
            BindingContext = new ProveedorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ProveedorPage = this;
        }
    }
}