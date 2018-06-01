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
	public partial class CajaInicializadaItemPage : ContentPage
	{
		public CajaInicializadaItemPage ()
		{
			InitializeComponent ();
            CajasInicializadasViewModel cajasInicializadasViewModel = CajasInicializadasViewModel.GetInstance();
            BindingContext = cajasInicializadasViewModel.CurrentCajasInicializadas;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.CajaInicializadaItemPage = this;
        }
    }
}