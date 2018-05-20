using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages.ProductoItemPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UnidadMedidaItemPage : ContentPage
	{
		public UnidadMedidaItemPage ()
		{
			InitializeComponent ();

            UnidadMedidaViewModel unidadMedidaViewModel = UnidadMedidaViewModel.GetInstance();
            BindingContext = unidadMedidaViewModel.CurrentUnidadMedida;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.UnidadMedidaItemPage = this;
        }
    }
}