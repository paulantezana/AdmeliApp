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
    public class DocIdentificacionItemViewModel : DocIdentificacion
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
        public DocIdentificacionItemViewModel()
        {
            // Estados
            this.IsRunning = false;
            this.IsEnabled = true;
            this.IconToggleOptions = "expandToggle_icon.png"; //Icono por defecto para expandir la item de la lista
            this.estado = 1;
            this.LoadTipoDocumento();
        }
        #endregion

        #region ================================== LISTS ==================================
        // =======================================================================================
        // Listar tipo documento ----------------------------------------------------------------
        // =======================================================================================
        private TipoDocumentoI _TipoDocumentoSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public TipoDocumentoI TipoDocumentoSelectedItem
        {
            get { return this._TipoDocumentoSelectedItem; }
            set
            {
                SetValue(ref this._TipoDocumentoSelectedItem, value);
            }
        }

        private List<TipoDocumentoI> _TipoDocumentoItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<TipoDocumentoI> TipoDocumentoItems
        {
            get { return this._TipoDocumentoItems; }
            set { SetValue(ref this._TipoDocumentoItems, value); }
        }
        #endregion

        #region ======================================== LOADS ========================================
        private void LoadTipoDocumento()
        {
            TipoDocumentoItems = new List<TipoDocumentoI>()
            {
                new TipoDocumentoI()
                {
                    idTipoDocumento = "1",
                    nombre = "Natural"
                },
                new TipoDocumentoI()
                {
                    idTipoDocumento = "2",
                    nombre = "Jurídico"
                },
            };
        }
        #endregion

        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteEditar()
        {
            DocIdentificacionViewModel docIdentificacionViewModel = DocIdentificacionViewModel.GetInstance();
            docIdentificacionViewModel.SetCurrentDocIdentificacion(this);
            this.Nuevo = false; /// Importante indicaque se modificara el registro actual
            this.DeleteIsEnabled = true;
            App.DocIdentificacionPage.Navigation.PushAsync(new DocIdentificacionItemPage()); // Navegacion

            // Establecer valores al modificar
            TipoDocumentoSelectedItem = TipoDocumentoItems.Find(x => x.idTipoDocumento == this.tipoDocumento);
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
                //DocIdentificacionViewModel.GetInstance().ExecuteRefresh();
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
                /// validacion de los campos
                if (string.IsNullOrEmpty(this.nombre))
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Campo nombre obligatoria", "Aceptar");
                    return;
                }

                if (this.numeroDigitos == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Campo digitos obligatoria", "Aceptar");
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
                    // localhost:8080/admeli/xcore2/xcore/services.php/documentoidentificacion/guardar
                    Response response = await webService.POST<DocIdentificacion, Response>("documentoidentificacion", "guardar", (DocIdentificacion)this);
                    await App.Current.MainPage.DisplayAlert("Guardar", response.Message, "Aceptar");
                }
                else
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/documentoidentificacion/modificar
                    Response response = await webService.POST<DocIdentificacion, Response>("documentoidentificacion", "modificar", (DocIdentificacion)this);
                    await App.Current.MainPage.DisplayAlert("Modificar", response.Message, "Aceptar");
                }

                // Refrescar y regresar a la pagina anterior
                DocIdentificacionViewModel.GetInstance().ExecuteRefresh();
                await App.DocIdentificacionItemPage.Navigation.PopAsync();
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
                //DocIdentificacionViewModel.GetInstance().ExecuteRefresh();
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

    public class TipoDocumentoI
    {
        public string idTipoDocumento { get; set; }
        public string nombre { get; set; }
    }
}
