﻿using AdmeliApp.ViewModel;
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
	public partial class MonedaPage : ContentPage
	{
		public MonedaPage ()
		{
			InitializeComponent ();
            this.BindingContext = new MonedaViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MonedaPage = this;
        }
    }
}