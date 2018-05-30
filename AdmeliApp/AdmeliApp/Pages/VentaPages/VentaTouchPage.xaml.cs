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
	public partial class VentaTouchPage : ContentPage
	{
		public VentaTouchPage ()
		{
			InitializeComponent ();
            this.BindingContext = new VentaTouchViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.VentaTouchPage = this;
        }
    }
}