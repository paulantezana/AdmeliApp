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
	public partial class GrupoClientePage : ContentPage
	{
		public GrupoClientePage ()
		{
			InitializeComponent ();
            this.BindingContext = new GrupoClienteViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.GrupoClientePage = this;
        }
    }
}