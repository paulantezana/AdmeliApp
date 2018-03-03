using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.New;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ItemViewModel
{
    public class MarcaItemViewModel : Marca
    {
        internal WebService webService = new WebService();

        private ICommand _GuardarCommand;
        public ICommand GuardarCommand =>
            _GuardarCommand ?? (_GuardarCommand = new Command(() => ExecuteGuardarAsync()));

        private ICommand _EditarCommand;
        public ICommand EditarCommand =>
            _EditarCommand ?? (_EditarCommand = new Command(() => ExecuteEditar()));

        private ICommand _AnularCommand;
        public ICommand AnularCommand =>
            _AnularCommand ?? (_AnularCommand = new Command(() => ExecuteAnular()));

        private void ExecuteEditar()
        {
            App.MarcaPage.Navigation.PushAsync(new NewMarcaPage());
        }

        private async void ExecuteAnular()
        {
            try
            {
                /// Verificacion si el registro esta anulado
                if(this.Estado == 0)
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

                // Mensaje de respuesta
                await App.Current.MainPage.DisplayAlert("Anular", response.Message, "Aceptar");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void ExecuteGuardarAsync()
        {
            try
            {
                // localhost:8080/admeli/xcore2/xcore/services.php/marca/guardar
                Marca marca = new Marca();
                marca.NombreMarca = this.NombreMarca;
                marca.SitioWeb = this.SitioWeb;
                marca.Descripcion = this.Descripcion;
                marca.Estado = this.Estado;
                marca.CaptionImagen = "";
                marca.UbicacionLogo = "";
                marca.TieneRegistros = "";


                Response response = await webService.POST<Marca, Response>("marca", "guardar", marca);
                await App.Current.MainPage.DisplayAlert("Guardar",response.Message,"Aceptar");
                marca.NombreMarca = string.Empty;
                marca.SitioWeb = string.Empty;
                marca.Descripcion = string.Empty;
                marca.Estado = 1;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
