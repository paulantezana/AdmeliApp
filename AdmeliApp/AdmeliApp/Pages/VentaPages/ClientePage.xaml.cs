﻿using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.VentaPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientePage : ContentPage
	{
		public ClientePage ()
		{
			InitializeComponent ();
            this.BindingContext = new ClienteViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ClientePage = this;
        }
    }
}