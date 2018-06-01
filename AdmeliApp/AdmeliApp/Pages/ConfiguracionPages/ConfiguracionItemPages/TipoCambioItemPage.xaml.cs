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
	public partial class TipoCambioItemPage : ContentPage
	{
		public TipoCambioItemPage ()
		{
			InitializeComponent ();
            TipoCambioViewModel tipoCambioViewModel = TipoCambioViewModel.GetInstance();
            BindingContext = tipoCambioViewModel.CurrentTipoCambio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.TipoCambioItemPage = this;
        }
    }
}