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
	public partial class ProductosPage : ContentPage
	{
		public ProductosPage ()
		{
			InitializeComponent ();
            BindingContext = new ProductoViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ProductosPage = this;
        }
    }
}