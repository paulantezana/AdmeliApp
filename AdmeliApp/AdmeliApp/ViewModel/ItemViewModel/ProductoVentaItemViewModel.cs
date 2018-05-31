using AdmeliApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class ProductoVentaItemViewModel : ProductoVenta
    {
        private bool _IsVisiblePrecioVenta;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool IsVisiblePrecioVenta
        {
            get { return this._IsVisiblePrecioVenta; }
            set { SetValue(ref this._IsVisiblePrecioVenta, value); }
        }

        private bool _VentaOptionsIsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool VentaOptionsIsVisible
        {
            get { return _VentaOptionsIsVisible; }
            set { SetValue(ref _VentaOptionsIsVisible, value); }
        }


        private ICommand _AddToCartCommand;
        public ICommand AddToCartCommand =>
            _AddToCartCommand ?? (_AddToCartCommand = new Command(() => ExecuteAddToCart()));

        private void ExecuteAddToCart()
        {
            VentaOptionsIsVisible = !VentaOptionsIsVisible;
        }
    }
}
