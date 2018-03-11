using AdmeliApp.Helpers;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace AdmeliApp.ViewModel
{
    public class HomeViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        List<Entry> ventaMensualEntity = new List<Entry>();
       // List<Entry> ingresoMonedaEntity = new List<Entry>();

        private Chart _VentaMensualChart;
        public Chart VentaMensualChart
        {
            get { return this._VentaMensualChart; }
            set { SetValue(ref this._VentaMensualChart, value); }
        }

        //private Chart _IngresoMonedaChart;
        //public Chart IngresoMonedaChart
        //{
        //    get { return this._IngresoMonedaChart; }
        //    set { SetValue(ref this._IngresoMonedaChart, value); }
        //}
        
        #region ============================== CONSTRUCTOR ==============================
        public HomeViewModel()
        {
            this.IsRunning = false;
            this.IsEnabled = true;

            loadVentasMensules();
            //loadIngresoMoneda();
        }
        #endregion

        private async void loadVentasMensules()
        {
            try
            {
                /// Actualizando los estados
                this.IsRunning = true;
                this.IsEnabled = false;

                // localhost:8080/admeli/xcore/services.php/ventaspormes
                List<UltimasVentas> ventas = await webService.GET<List<UltimasVentas>>("ventaspormes");
                foreach (UltimasVentas item in ventas)
                {
                    ventaMensualEntity.Add(new Entry(int.Parse(item.total))
                    {
                        Color = SKColor.Parse("#36A2EB"),
                        Label = item.dia,
                        ValueLabel = item.total,
                    });
                }
                VentaMensualChart = new LineChart { Entries = ventaMensualEntity };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", ex.Message + ":(", "Aceptar");
            }
            finally
            {
                this.IsRunning = false;
                this.IsEnabled = true;
            }
        }

        //private async void loadIngresoMoneda()
        //{
        //    try
        //    {
        //        /// Actualizando los estados
        //        this.IsRunning = true;
        //        this.IsEnabled = false;

        //        // localhost:8080/admeli/xcore/services.php/ventaspormes
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Alerta", ex.Message + ":(", "Aceptar");
        //    }
        //    finally
        //    {
        //        this.IsRunning = false;
        //        this.IsEnabled = true;
        //    }
        //}
    }
    public class UltimasVentas
    {
        public string dia { get; set; }
        public dynamic idVenta { get; set; }
        public string total { get; set; }
    }
}
