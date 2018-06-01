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
	public partial class PersonalItemPage : ContentPage
	{
		public PersonalItemPage ()
		{
			InitializeComponent ();
            PersonalViewModel personalViewModel = PersonalViewModel.GetInstance();
            BindingContext = personalViewModel.CurrentPersonal;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.PersonalItemPage = this;
        }
    }
}