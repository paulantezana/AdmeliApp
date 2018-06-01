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
	public partial class DocIdentificacionItemPage : ContentPage
	{
		public DocIdentificacionItemPage ()
		{
			InitializeComponent ();
            DocIdentificacionViewModel docIdentificacionViewModel = DocIdentificacionViewModel.GetInstance();
            BindingContext = docIdentificacionViewModel.CurrentDocIdentificacion;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.DocIdentificacionItemPage = this;
        }
    }
}