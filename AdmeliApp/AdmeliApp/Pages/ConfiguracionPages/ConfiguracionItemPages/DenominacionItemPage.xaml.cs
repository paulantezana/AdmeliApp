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
	public partial class DenominacionItemPage : ContentPage
	{
		public DenominacionItemPage ()
		{
			InitializeComponent ();
            DenominacionViewModel denominacionViewModel = DenominacionViewModel.GetInstance();
            BindingContext = denominacionViewModel.CurrentDenominacion;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.DenominacionItemPage = this;
        }
    }
}