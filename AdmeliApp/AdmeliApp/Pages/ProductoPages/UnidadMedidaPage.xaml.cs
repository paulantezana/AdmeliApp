﻿using AdmeliApp.ViewModel;
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
	public partial class UnidadMedidaPage : ContentPage
	{
		public UnidadMedidaPage ()
		{
			InitializeComponent ();
            BindingContext = new UnidadMedidaViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.UnidadMedidaPage = this;
        }
    }
}