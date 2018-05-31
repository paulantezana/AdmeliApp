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

        private bool _ToggleOptionsIsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool ToggleOptionsIsVisible
        {
            get { return this._ToggleOptionsIsVisible; }
            set { SetValue(ref this._ToggleOptionsIsVisible, value); }
        }

        #region =================================== COMMANDS ===================================
        private ICommand _ToggleOptionsCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand ToggleOptionsCommand =>
            _ToggleOptionsCommand ?? (_ToggleOptionsCommand = new Command(() => ExecuteToggleOptions()));
        #endregion

        #region ================================= EXECUTE COMMANDS =================================
        private void ExecuteToggleOptions()
        {
            this.ToggleOptionsIsVisible = !this.ToggleOptionsIsVisible;
        } 
        #endregion
    }
}
