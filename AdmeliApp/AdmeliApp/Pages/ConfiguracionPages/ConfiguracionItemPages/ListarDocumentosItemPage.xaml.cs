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
	public partial class ListarDocumentosItemPage : ContentPage
	{
		public ListarDocumentosItemPage ()
		{
			InitializeComponent ();
            ListarDocumentosViewModel listarDocumentosViewModel = ListarDocumentosViewModel.GetInstance();
            BindingContext = listarDocumentosViewModel.CurrentListarDocumentos;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarDocumentosItemPage = this;
        }
    }
}