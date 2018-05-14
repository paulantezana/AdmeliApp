using AdmeliApp.Helpers;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class DatosEmpresaViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        private DatosGenerales _DatosGeneralesData;
        public DatosGenerales DatosGeneralesData
        {
            get { return this._DatosGeneralesData; }
            set { SetValue(ref this._DatosGeneralesData, value); }
        }

        private ConfiguracionGeneral _ConfiguracionGeneralData;
        public ConfiguracionGeneral ConfiguracionGeneralData
        {
            get { return this._ConfiguracionGeneralData; }
            set { SetValue(ref this._ConfiguracionGeneralData, value); }
        }

        public DatosEmpresaViewModel()
        {
            LoadDatosGenerales();
        }

        private async void LoadDatosGenerales()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/generales
                List<DatosGenerales> listData = await webService.GET<List<DatosGenerales>>("generales");
                DatosGeneralesData = listData[0];

                // www.lineatienda.com/services.php/configeneral
                List<ConfiguracionGeneral> list = await webService.GET<List<ConfiguracionGeneral>>("configeneral");
                ConfiguracionGeneralData = list[0];
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

    }
}
