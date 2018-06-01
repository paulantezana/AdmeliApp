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
    public class ImpuestoItemViewModel : Impuesto
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
        public ImpuestoItemViewModel()
        {
            // Estados
            this.IsRunning = false;
            this.IsEnabled = true;
            this.IconToggleOptions = "expandToggle_icon.png"; //Icono por defecto para expandir la item de la lista
            this.estado = 1;
        }
        #endregion

        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteEditar()
        {
            ImpuestoViewModel impuestoViewModel = ImpuestoViewModel.GetInstance();
            impuestoViewModel.SetCurrentImpuesto(this);
            this.Nuevo = false; /// Importante indicaque se modificara el registro actual
            this.DeleteIsEnabled = true;
            App.ImpuestoPage.Navigation.PushAsync(new ImpuestoItemPage()); // Navegacion
        }

        private async void ExecuteAnular()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                // Verificacion si el registro esta anulado
                if (this.estado == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Anular", "Este registro ya esta anulado \n" + this.nombreImpuesto, "Aceptar");
                    return;
                }

                // pregunta al usuario (Confirmacion)
                if (await App.Current.MainPage.DisplayAlert("Anular", "¿esta seguro de anular este registro? \n" + this.nombreImpuesto, "Aceptar", "Cancelar") == false) return;

                // Preparando el objeto para enviar
                Impuesto impuesto = new Impuesto();
                impuesto.idImpuesto = this.idImpuesto;

                // Ejecutando el webservice
                // localhost:8080/admeli/xcore2/xcore/services.php/impuesto/desactivar
                Response response = await webService.POST<Impuesto, Response>("impuesto", "desactivar", impuesto);

                // Message response
                await App.Current.MainPage.DisplayAlert("Anular", response.Message, "Aceptar");

                // Refrescar la lista
                ImpuestoViewModel.GetInstance().ExecuteRefresh();
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
                // validacion de los campos
                if (string.IsNullOrEmpty(this.nombreImpuesto))
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Campo nombre impuesto obligatoria", "Aceptar");
                    return;
                }

                if (string.IsNullOrEmpty(this.valorImpuesto))
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Campo valor impuesto obligatoria", "Aceptar");
                    return;
                }

                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                // Preparando el objeto para enviar
                if (this.Nuevo)
                {
                }

                if (this.Nuevo)
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/impuesto/guardar
                    Response response = await webService.POST<Impuesto, Response>("impuesto", "guardar", (Impuesto)this);
                    await App.Current.MainPage.DisplayAlert("Guardar", response.Message, "Aceptar");
                }
                else
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/impuesto/modificar
                    Response response = await webService.POST<Impuesto, Response>("impuesto", "modificar", (Impuesto)this);
                    await App.Current.MainPage.DisplayAlert("Modificar", response.Message, "Aceptar");
                }

                // Refrescar y regresar a la pagina anterior
                ImpuestoViewModel.GetInstance().ExecuteRefresh();
                await App.ImpuestoItemPage.Navigation.PopAsync();
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

                // pregunta al usuario (Confirmacion)
                if (await App.Current.MainPage.DisplayAlert("Eliminar", "¿esta seguro de eliminar este registro? \n" + this.nombreImpuesto, "Aceptar", "Cancelar") == false) return;

                // Preparando el objeto para enviar
                Impuesto impuesto = new Impuesto();
                impuesto.idImpuesto = this.idImpuesto;

                // localhost:8080/admeli/xcore2/xcore/services.php/impuesto/eliminar
                Response response = await webService.POST<Impuesto, Response>("impuesto", "eliminar", impuesto);
                await App.Current.MainPage.DisplayAlert("Eliminar", response.Message, "Aceptar");

                // Refrescar la lista
                ImpuestoViewModel.GetInstance().ExecuteRefresh();
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
