using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ItemPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class MarcaItemViewModel : Marca
    {
        internal WebService webService = new WebService();
        public bool Nuevo { get; set; }

        #region ================================= COMMANDS =================================
        private ICommand _GuardarCommand;
        internal ICommand GuardarCommand =>
            _GuardarCommand ?? (_GuardarCommand = new Command(() => ExecuteGuardarAsync()));

        private ICommand _EditarCommand;
        internal ICommand EditarCommand =>
            _EditarCommand ?? (_EditarCommand = new Command(() => ExecuteEditar()));

        private ICommand _AnularCommand;
        internal ICommand AnularCommand =>
            _AnularCommand ?? (_AnularCommand = new Command(() => ExecuteAnular()));
        #endregion

        #region ================================ CONSTRUCTOR ================================
        public MarcaItemViewModel()
        {
            // Estados
            this.IsRunning = false;
            this.IsEnabled = true;
        } 
        #endregion

        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteEditar()
        {
            MarcaViewModel marcaViewModel = MarcaViewModel.GetInstance();
            marcaViewModel.SetCurrentMarca(this);
            this.Nuevo = false; /// Importante indicaque se modificara el registro actual
            App.MarcaPage.Navigation.PushAsync(new MarcaItemPage()); // Navegacion
        }

        private async void ExecuteAnular()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                /// Verificacion si el registro esta anulado
                if (this.Estado == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Anular", "Este registro ya esta anulado \n" + this.NombreMarca, "Aceptar");
                    return;
                }

                /// pregunta al usuario (Confirmacion)
                if (await App.Current.MainPage.DisplayAlert("Anular", "¿esta seguro de anular este registro? \n" + this.NombreMarca, "Aceptar", "Cancelar") == false) return;

                /// Preparando el objeto para enviar
                Marca marca = new Marca();
                marca.IdMarca = this.IdMarca;

                /// Ejecutando el webservice
                // localhost:8080/admeli/xcore2/xcore/services.php/marca/desactivar
                Response response = await webService.POST<Marca, Response>("marca", "desactivar", marca);

                // Message response
                await App.Current.MainPage.DisplayAlert("Anular", response.Message, "Aceptar");
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
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                // Preparando el objeto para enviar
                //Marca marca = new Marca();
                if (this.Nuevo)
                {
                    this.CaptionImagen = "";
                    this.UbicacionLogo = "";
                    this.TieneRegistros = "";
                }
                /*else
                {
                    marca.IdMarca = IdMarca;
                }*/
               /* marca.NombreMarca = this.NombreMarca;
                marca.SitioWeb = this.SitioWeb;
                marca.Descripcion = this.Descripcion;
                marca.Estado = this.Estado;*/

                if (this.Nuevo)
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/marca/guardar
                    Response response = await webService.POST<Marca, Response>("marca", "guardar", (Marca)this);
                    await App.Current.MainPage.DisplayAlert("Guardar", response.Message, "Aceptar");
                }
                else
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/marca/modificar
                    Response response = await webService.POST<Marca, Response>("marca", "modificar", (Marca)this);
                    await App.Current.MainPage.DisplayAlert("Modificar", response.Message, "Aceptar");
                }

                // Limpiando los campos
                /*marca.NombreMarca = string.Empty;
                marca.SitioWeb = string.Empty;
                marca.Descripcion = string.Empty;
                marca.Estado = 1;*/
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
        #endregion
    }
}
