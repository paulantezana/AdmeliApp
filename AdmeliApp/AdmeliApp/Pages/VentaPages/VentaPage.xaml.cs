using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.VentaPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VentaPage : ContentPage
	{
		public VentaPage ()
		{
			InitializeComponent ();
            this.BindingContext = new VentaViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.VentaPage = this;
        }
    }
}