using AdmeliApp.MainViewModel;
using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriaPage : ContentPage
	{
		public CategoriaPage ()
		{
			InitializeComponent ();
            BindingContext = new CategoriaMainViewModel();
        }
	}
}