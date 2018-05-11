using AdmeliApp.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages.Util
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectableCategoriaPage : ContentPage
	{
		public SelectableCategoriaPage ()
		{
			InitializeComponent ();
            this.BindingContext = new SelectableCategoriaViewModel();
        }
	}
}