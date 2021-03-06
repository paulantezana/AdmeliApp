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
	public partial class DescuentoPage : ContentPage
	{
		public DescuentoPage ()
		{
			InitializeComponent ();
            this.BindingContext = new DescuentoViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.DescuentoPage = this;
        }
    }
}