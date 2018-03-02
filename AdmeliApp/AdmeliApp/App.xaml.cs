using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdmeliApp.Pages.ProductoPages;
using Xamarin.Forms;

namespace AdmeliApp
{
	public partial class App : Application
	{
        public static MarcaPage MarcaPage { get; internal set; }

        public App ()
		{
			InitializeComponent();

            MainPage = new AdmeliApp.Pages.Root.LoginPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
