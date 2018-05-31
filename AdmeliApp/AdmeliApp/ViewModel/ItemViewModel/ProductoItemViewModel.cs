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
        public bool Nuevo;

        private bool _DeleteIsEnabled;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool DeleteIsEnabled
        {
            get { return _DeleteIsEnabled; }
            set { SetValue(ref _DeleteIsEnabled, value); }
        }

        #region ================================== COMMANDS ==================================
        private ICommand _EliminarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand EliminarCommand =>
            _EliminarCommand ?? (_EliminarCommand = new Command(() => ExecuteEliminar()));

        private ICommand _ViewCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand ViewCommand =>
            _ViewCommand ?? (_ViewCommand = new Command(() => ExecuteView()));

        private ICommand _ToggleOptionsCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand ToggleOptionsCommand =>
            _ToggleOptionsCommand ?? (_ToggleOptionsCommand = new Command(() => ExecuteToggleOptions()));
        #endregion

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

        // ========================================================================================
        // Usado para mostras mas opciones en cada item del producto
        private bool _ToggleOptionsIsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool ToggleOptionsIsVisible
        {
            get { return this._ToggleOptionsIsVisible; }
            set { SetValue(ref this._ToggleOptionsIsVisible, value); }
        }

        // Usado para mostras mas opciones en cada item del producto
        private string _IconToggleOptions;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public string IconToggleOptions
        {
            get { return this._IconToggleOptions; }
            set { SetValue(ref this._IconToggleOptions, value); }
        }
        // ========================================================================================

        #region ====================================== CONSTRUCTOR ======================================
        public ProductoItemViewModel()
        {
            IsVisiblePrecioVenta = (App.asignacionPersonal.idAsignarPuntoVenta > 0) ? true : false;
            IsVisiblePrecioCompra = (App.asignacionPersonal.idAsignarPuntoCompra > 0) ? true : false;
            this.IconToggleOptions = "expandToggle_icon.png"; //Icono por defecto para expandir la item de la lista
        }
        #endregion

        #region ================================== EXECUTE COMMAND ==================================
        private void ExecuteView()
        {
            ProductoViewModel productoViewModel = ProductoViewModel.GetInstance();
            productoViewModel.SetCurrentProducto(this);
            App.ProductosPage.Navigation.PushAsync(new ProductoItemPage());
        }

        private async void ExecuteEliminar()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                if(this.enUso == true)
                {
                    // pregunta al usuario (Confirmacion)
                    if (await App.Current.MainPage.DisplayAlert("Inhabilitar", "¿Está seguro de inhabilitar este registro? \n" + this.nombreProducto, "Aceptar", "Cancelar") == false) return;
                    Producto currentProducto = new Producto(); //creando una instancia del objeto categoria
                    currentProducto.idProducto = idProducto;

                    //localhost:8085/ad_meli/xcore/services.php/producto/inhabilitar
                    Response response = await webService.POST<Producto, Response>("producto", "inhabilitar", currentProducto);
                    await App.Current.MainPage.DisplayAlert("Inhabilitar", response.Message, "Aceptar");
                }
                else
                {
                    // pregunta al usuario (Confirmacion)
                    if (await App.Current.MainPage.DisplayAlert("Eliminar", "¿Está seguro de eliminar este registro? \n" + this.nombreProducto, "Aceptar", "Cancelar") == false) return;
                    Producto currentProducto = new Producto(); //creando una instancia del objeto categoria
                    currentProducto.idProducto = idProducto;

                    // localhost/admeli/xcore/services.php/producto/eliminar
                    Response response = await webService.POST<Producto, Response>("producto", "eliminar", currentProducto);
                    await App.Current.MainPage.DisplayAlert("Eliminar", response.Message, "Aceptar");

                }

                // Refrescar la lista
                MarcaViewModel.GetInstance().ExecuteRefresh();
            }
            catch (Exception ex)
            {
                // Error message
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                // Estados
                this.IsRunning = false;
                this.IsEnabled = true;
            }
        }

        private void ExecuteToggleOptions()
        {
            this.ToggleOptionsIsVisible = !this.ToggleOptionsIsVisible;
            IconToggleOptions = (ToggleOptionsIsVisible) ? "collapseToggle_icon.png" : "expandToggle_icon.png"; //Cambiando los iconos en tiempo real
        }
        #endregion
    }
}
