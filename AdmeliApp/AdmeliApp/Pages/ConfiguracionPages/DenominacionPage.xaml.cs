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
	public partial class DenominacionPage : ContentPage
	{
		public DenominacionPage ()
		{
			InitializeComponent ();
            this.BindingContext = new DenominacionViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.DenominacionPage = this;
        }
    }
}