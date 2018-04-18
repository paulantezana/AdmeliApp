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
	public partial class ListarDocumentosPage : ContentPage
	{
		public ListarDocumentosPage ()
		{
			InitializeComponent ();
            this.BindingContext = new TipoDocumentoViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.TipoDocumentoPage = this;
        }
    }
}