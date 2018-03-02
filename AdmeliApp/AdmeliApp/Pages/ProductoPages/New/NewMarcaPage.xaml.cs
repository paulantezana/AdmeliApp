﻿using AdmeliApp.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages.New
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewMarcaPage : ContentPage
	{
		public NewMarcaPage ()
		{
			InitializeComponent ();
            this.BindingContext = new MarcaItemViewModel();
        }
	}
}