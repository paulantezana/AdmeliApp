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
	public partial class SucursalItemPage : ContentPage
	{
		public SucursalItemPage ()
		{
			InitializeComponent ();
            SucursalViewModel sucursalViewModel = SucursalViewModel.GetInstance();
            BindingContext = sucursalViewModel.CurrentSucursal;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SucursalItemPage = this;
        }
    }
}