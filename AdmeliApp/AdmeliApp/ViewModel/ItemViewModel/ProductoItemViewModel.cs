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

        private bool _IsVisiblePrecioCompra;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool IsVisiblePrecioCompra
        {
            get { return this._IsVisiblePrecioCompra; }
            set { SetValue(ref this._IsVisiblePrecioCompra, value); }
        }

        private bool _IsVisiblePrecioVenta;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool IsVisiblePrecioVenta
        {
            get { return this._IsVisiblePrecioVenta; }
            set { SetValue(ref this._IsVisiblePrecioVenta, value); }
        }

        private bool _IsVisibleStock;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool IsVisibleStock
        {
            get { return this._IsVisibleStock; }
            set { SetValue(ref this._IsVisibleStock, value); }
        }


        public ProductoItemViewModel()
        {
            IsVisiblePrecioVenta = (App.asignacionPersonal.idAsignarPuntoVenta > 0) ? true : false;
            IsVisiblePrecioCompra = (App.asignacionPersonal.idAsignarPuntoCompra > 0) ? true : false;
        }

        private void ExecuteView()
        {
            ProductoViewModel productoViewModel = ProductoViewModel.GetInstance();
            productoViewModel.SetCurrentProducto(this);
            App.ProductosPage.Navigation.PushAsync(new ProductoItemPage());
        }
    }
}
