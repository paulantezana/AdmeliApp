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
	public partial class AsignarCorrelativoItemPage : ContentPage
	{
		public AsignarCorrelativoItemPage ()
		{
			InitializeComponent ();
            AsignarCorrelativoViewModel asignarCorrelativoViewModel = AsignarCorrelativoViewModel.GetInstance();
            BindingContext = asignarCorrelativoViewModel.CurrentAsignarCorrelativo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.AsignarCorrelativoItemPage = this;
        }
    }
}