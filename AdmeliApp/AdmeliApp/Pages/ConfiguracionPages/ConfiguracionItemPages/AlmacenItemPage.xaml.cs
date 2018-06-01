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
	public partial class AlmacenItemPage : ContentPage
	{
		public AlmacenItemPage ()
		{
			InitializeComponent ();
            AlmacenViewModel almacenViewModel = AlmacenViewModel.GetInstance();
            BindingContext = almacenViewModel.CurrentAlmacen;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.AlmacenItemPage = this;
        }
    }
}