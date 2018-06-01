using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ConfiguracionPages.ConfiguracionItemPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PuntoVentaItemPage : ContentPage
	{
		public PuntoVentaItemPage ()
		{
			InitializeComponent ();
            PuntoVentaViewModel puntoVentaViewModel = PuntoVentaViewModel.GetInstance();
            BindingContext = puntoVentaViewModel.CurrentPuntoVenta;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.PuntoVentaItemPage = this;
        }
    }
}