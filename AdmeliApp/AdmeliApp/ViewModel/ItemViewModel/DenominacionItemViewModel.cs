using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ConfiguracionPages.ConfiguracionItemPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class DenominacionItemViewModel : Denominacion
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

        private bool _ToggleOptionsIsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool ToggleOptionsIsVisible
        {
            get { return this._ToggleOptionsIsVisible; }
            set { SetValue(ref this._ToggleOptionsIsVisible, value); }
        }

        private string _IconToggleOptions;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public string IconToggleOptions
        {
            get { return this._IconToggleOptions; }
            set { SetValue(ref this._IconToggleOptions, value); }
        }

        #region ================================= COMMANDS =================================
        private ICommand _GuardarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand GuardarCommand =>
            _GuardarCommand ?? (_GuardarCommand = new Command(() => ExecuteGuardarAsync()));

        private ICommand _EditarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand EditarCommand =>
            _EditarCommand ?? (_EditarCommand = new Command(() => ExecuteEditar()));

        private ICommand _AnularCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand AnularCommand =>
            _AnularCommand ?? (_AnularCommand = new Command(() => ExecuteAnular()));

        private ICommand _EliminarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand EliminarCommand =>
            _EliminarCommand ?? (_EliminarCommand = new Command(() => ExecuteEliminar()));

        private ICommand _ToggleOptionsCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand ToggleOptionsCommand =>
            _ToggleOptionsCommand ?? (_ToggleOptionsCommand = new Command(() => ExecuteToggleOptions()));
        #endregion

        #region ================================ CONSTRUCTOR ================================
        public DenominacionItemViewModel()
        {
            // Estados
            this.IsRunning = false;
            this.IsEnabled = true;
            this.IconToggleOptions = "expandToggle_icon.png"; //Icono por defecto para expandir la item de la lista
            this.estado = 1;
            RootLoad(); 
        }
        #endregion

        #region =================================== LISTS ===================================
        // =======================================================================================
        // Listar Moneda ----------------------------------------------------------------
        // =======================================================================================
        private Moneda _MonedaSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Moneda MonedaSelectedItem
        {
            get { return this._MonedaSelectedItem; }
            set
            {
                SetValue(ref this._MonedaSelectedItem, value);
            }
        }

        private List<Moneda> _MonedaItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Moneda> MonedaItems
        {
            get { return this._MonedaItems; }
            set { SetValue(ref this._MonedaItems, value); }
        }

        // =======================================================================================
        // Listar tipo moneda ----------------------------------------------------------------
        // =======================================================================================
        private TipoMonedaI _TipoMonedaSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public TipoMonedaI TipoMonedaSelectedItem
        {
            get { return this._TipoMonedaSelectedItem; }
            set
            {
                SetValue(ref this._TipoMonedaSelectedItem, value);
            }
        }

        private List<TipoMonedaI> _TipoMonedaItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<TipoMonedaI> TipoMonedaItems
        {
            get { return this._TipoMonedaItems; }
            set { SetValue(ref this._TipoMonedaItems, value); }
        }
        #endregion

        #region ================================== LOADS ==================================
        private void RootLoad()
        {
            LoadMoneda();
            LoadTipoMoneda();
        }
        private void LoadTipoMoneda()
        {
            TipoMonedaItems = new List<TipoMonedaI>()
            {
                new TipoMonedaI()
                {
                    idTipoMoneda = "1",
                    nombre = "BILLETE"
                },
                new TipoMonedaI()
                {
                    idTipoMoneda = "2",
                    nombre = "MONEDA"
                },
            };
        }

        private async void LoadMoneda()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/monedas/estado/1
                List<Moneda> list = await webService.GET<List<Moneda>>("monedas", String.Format("estado/{0}", estado));

                // www.lineatienda.com/services.php/categorias21/-1
                int MonedaID = (idMoneda > 0) ? idMoneda : -1;
                List<Moneda> datos = await webService.GET<List<Moneda>>("monedas", String.Format("estado/{0}", estado));
                MonedaItems = datos;

                MonedaSelectedItem = datos.Find(c => c.idMoneda == this.idMoneda); // Selecciona la moneda
                TipoMonedaSelectedItem = TipoMonedaItems.Find(x => x.idTipoMoneda == "1");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                this.IsRefreshing = false;
                this.IsEnabled = true;
            }
        }
        #endregion

        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteEditar()
        {
            DenominacionViewModel denominacionViewModel = DenominacionViewModel.GetInstance();
            denominacionViewModel.SetCurrentDenominacion(this);
            this.Nuevo = false; /// Importante indicaque se modificara el registro actual
            this.DeleteIsEnabled = true;
            App.DenominacionPage.Navigation.PushAsync(new DenominacionItemPage()); // Navegacion

            // Editar seleccionar los datos en el picker
            MonedaSelectedItem = MonedaItems.Find(m => m.idMoneda == this.idMoneda);
            TipoMonedaSelectedItem = TipoMonedaItems.Find(m => m.idTipoMoneda == this.tipoMoneda);
        }

        private async void ExecuteAnular()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                ///// Verificacion si el registro esta anulado
                //if (this.Estado == 0)
                //{
                //    await App.Current.MainPage.DisplayAlert("Anular", "Este registro ya esta anulado \n" + this.NombreMarca, "Aceptar");
                //    return;
                //}

                ///// pregunta al usuario (Confirmacion)
                //if (await App.Current.MainPage.DisplayAlert("Anular", "¿esta seguro de anular este registro? \n" + this.NombreMarca, "Aceptar", "Cancelar") == false) return;

                ///// Preparando el objeto para enviar
                //Marca marca = new Marca();
                //marca.IdMarca = this.IdMarca;

                ///// Ejecutando el webservice
                //// localhost:8080/admeli/xcore2/xcore/services.php/marca/desactivar
                //Response response = await webService.POST<Marca, Response>("marca", "desactivar", marca);

                //// Message response
                //await App.Current.MainPage.DisplayAlert("Anular", response.Message, "Aceptar");

                //// Refrescar la lista
                //MarcaViewModel.GetInstance().ExecuteRefresh();
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

        private async void ExecuteGuardarAsync()
        {
            try
            {
                ///// validacion de los campos
                //if (string.IsNullOrEmpty(this.NombreMarca))
                //{
                //    await Application.Current.MainPage.DisplayAlert("Alerta", "Campo obligatoria", "Aceptar");
                //    return;
                //}

                //// Estados
                //this.IsRunning = true;
                //this.IsEnabled = false;

                //// Preparando el objeto para enviar
                //if (this.Nuevo)
                //{
                //    this.CaptionImagen = "";
                //    this.UbicacionLogo = "";
                //    this.TieneRegistros = "";
                //}

                //if (this.Nuevo)
                //{
                //    // localhost:8080/admeli/xcore2/xcore/services.php/marca/guardar
                //    Response response = await webService.POST<Marca, Response>("marca", "guardar", (Marca)this);
                //    await App.Current.MainPage.DisplayAlert("Guardar", response.Message, "Aceptar");
                //}
                //else
                //{
                //    // localhost:8080/admeli/xcore2/xcore/services.php/marca/modificar
                //    Response response = await webService.POST<Marca, Response>("marca", "modificar", (Marca)this);
                //    await App.Current.MainPage.DisplayAlert("Modificar", response.Message, "Aceptar");
                //}

                //// Refrescar y regresar a la pagina anterior
                //MarcaViewModel.GetInstance().ExecuteRefresh();
                //await App.MarcaItemPage.Navigation.PopAsync();
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

        private async void ExecuteEliminar()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                ///// pregunta al usuario (Confirmacion)
                //if (await App.Current.MainPage.DisplayAlert("Eliminar", "¿esta seguro de eliminar este registro? \n" + this.NombreMarca, "Aceptar", "Cancelar") == false) return;

                //// localhost:8080/admeli/xcore2/xcore/services.php/marca/eliminar
                //Response response = await webService.POST<Marca, Response>("marca", "eliminar", (Marca)this);
                //await App.Current.MainPage.DisplayAlert("Eliminar", response.Message, "Aceptar");

                //// Refrescar la lista
                //MarcaViewModel.GetInstance().ExecuteRefresh();
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

    public class TipoMonedaI
    {
        public string idTipoMoneda { get; set; }
        public string nombre { get; set; }
    }
}
