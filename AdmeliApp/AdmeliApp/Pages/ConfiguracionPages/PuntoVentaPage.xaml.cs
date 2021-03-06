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
	public partial class PuntoVentaPage : ContentPage
	{
		public PuntoVentaPage ()
		{
			InitializeComponent ();
            this.BindingContext = new PuntoVentaViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.PuntoVentaPage = this;
        }
    }
}