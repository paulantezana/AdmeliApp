using AdmeliApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
