using AdmeliApp.Helpers;
using AdmeliApp.Model;
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

        private ICommand _EliminarCommand;
        public ICommand EliminarCommand =>
            _EliminarCommand ?? (_EliminarCommand = new Command(() => ExecuteEliminarAsync()));

        private async void ExecuteGuardarAsync()
        {
            try
            {
                // localhost:8080/admeli/xcore2/xcore/services.php/marca/guardar
                Marca marca = new Marca();
                marca.NombreMarca = this.NombreMarca;
                marca.SitioWeb = this.SitioWeb;
                marca.Descripcion = this.Descripcion;
                marca.Estado = 1;
                marca.CaptionImagen = "";
                marca.UbicacionLogo = "";
                marca.TieneRegistros = "";


                Response response = await webService.POST<Marca, Response>("marca", "guardar", marca);
                await App.Current.MainPage.DisplayAlert("Guardar",response.Message,"Aceptar");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void ExecuteEliminarAsync()
        {
            throw new NotImplementedException();
        }
    }
}
