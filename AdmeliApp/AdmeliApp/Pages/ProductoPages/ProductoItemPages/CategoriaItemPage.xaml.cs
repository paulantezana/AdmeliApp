﻿using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages.ProductoItemPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriaItemPage : ContentPage
	{
		public CategoriaItemPage ()
		{
			InitializeComponent ();

            CategoriaViewModel categoriaViewModel = CategoriaViewModel.GetInstance();
            BindingContext = categoriaViewModel.CurrentCategoria;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.CategoriaItemPage = this;
        }
    }
}