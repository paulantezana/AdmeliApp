using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ConfiguracionPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CajaInicializadaPage : ContentPage
	{
		public CajaInicializadaPage ()
		{
			InitializeComponent ();
            this.BindingContext = new CajasInicializadasViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.CajaInicializadaPage = this;
        }
    }
}