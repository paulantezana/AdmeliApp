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
	public partial class GrupoClienteItemPage : ContentPage
	{
		public GrupoClienteItemPage ()
		{
			InitializeComponent ();
            GrupoClienteViewModel grupoClienteViewModel = GrupoClienteViewModel.GetInstance();
            BindingContext = grupoClienteViewModel.CurrentGrupoCliente;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.GrupoClienteItemPage = this;
        }
    }
}