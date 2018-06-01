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
	public partial class ImpuestoItemPage : ContentPage
	{
		public ImpuestoItemPage ()
		{
			InitializeComponent ();
            ImpuestoViewModel impuestoViewModel = ImpuestoViewModel.GetInstance();
            BindingContext = impuestoViewModel.CurrentImpuesto;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ImpuestoItemPage = this;
        }
    }
}