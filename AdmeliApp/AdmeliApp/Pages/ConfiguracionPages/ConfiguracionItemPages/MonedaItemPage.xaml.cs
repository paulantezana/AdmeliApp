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
	public partial class MonedaItemPage : ContentPage
	{
		public MonedaItemPage ()
		{
			InitializeComponent ();
            MonedaViewModel monedaViewModel = MonedaViewModel.GetInstance();
            BindingContext = monedaViewModel.CurrentMoneda;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MonedaItemPage = this;
        }
    }
}