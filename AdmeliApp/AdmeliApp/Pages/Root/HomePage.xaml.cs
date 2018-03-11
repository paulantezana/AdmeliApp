using AdmeliApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.Root
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
            this.BindingContext = new HomeViewModel();
        }
	}
}