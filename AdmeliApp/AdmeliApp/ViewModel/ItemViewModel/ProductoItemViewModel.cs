using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class ProductoItemViewModel : Producto
    {
        internal WebService webService = new WebService();

        private ICommand _ViewCommand;
        public ICommand ViewCommand =>
            _ViewCommand ?? (_ViewCommand = new Command(() => ExecuteView()));

        private void ExecuteView()
        {
            ProductoViewModel productoViewModel = ProductoViewModel.GetInstance();
            productoViewModel.SetCurrentProducto(this);
            App.ProductosPage.Navigation.PushAsync(new ProductoItemPage());
        }
    }
}
