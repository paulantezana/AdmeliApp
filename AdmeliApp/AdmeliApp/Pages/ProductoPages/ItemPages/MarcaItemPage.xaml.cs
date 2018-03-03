using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages.ItemPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MarcaItemPage : ContentPage
	{
		public MarcaItemPage ()
		{
			InitializeComponent ();

            MarcaViewModel marcaViewModel = MarcaViewModel.GetInstance();
            BindingContext = marcaViewModel.CurrentMarca;
        }
	}
}