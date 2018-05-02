using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.Root
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigInitialPage : ContentPage
	{
		public ConfigInitialPage ()
		{
			InitializeComponent ();
            this.BindingContext = new ConfigInitialViewModel();
        }
	}
}