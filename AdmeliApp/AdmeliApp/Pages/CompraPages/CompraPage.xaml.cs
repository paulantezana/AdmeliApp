using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.CompraPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompraPage : ContentPage
	{
		public CompraPage ()
		{
			InitializeComponent ();
            BindingContext = new CompraViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.CompraPage = this;
        }
    }
}