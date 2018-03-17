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
	public partial class AlmacenPage : ContentPage
	{
		public AlmacenPage ()
		{
			InitializeComponent ();
            this.BindingContext = new AlmacenViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.AlmacenPage = this;
        }
    }
}