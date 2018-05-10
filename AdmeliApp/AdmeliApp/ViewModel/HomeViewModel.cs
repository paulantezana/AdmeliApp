using AdmeliApp.Helpers;
using AdmeliApp.Pages.CompraPages;
using AdmeliApp.Pages.ProductoPages;
using AdmeliApp.Pages.VentaPages;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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


        #region ======================= COMMANDS =======================
        private ICommand _ProductoCommand;
        public ICommand ProductoCommand =>
            _ProductoCommand ?? (_ProductoCommand = new Command(() => ExecuteProductoNav()));

        private ICommand _CompraCommand;
        public ICommand CompraCommand =>
            _CompraCommand ?? (_CompraCommand = new Command(() => ExecuteCompraNav()));

        private ICommand _VentaCommand;
        public ICommand VentaCommand =>
            _VentaCommand ?? (_VentaCommand = new Command(() => ExecuteVentaNav()));

        private ICommand _ProveedorCommand;
        public ICommand ProveedorCommand =>
            _ProveedorCommand ?? (_ProveedorCommand = new Command(() => ExecuteProveedorNav()));
        #endregion

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

        #region ======================= EXECUTE COMMANDS =======================
        private void ExecuteProductoNav()
        {
            Page page = new ProductosPage();
            page.Title = "Productos";
            App.RootPage.Detail = new NavigationPage(page);
        }

        private void ExecuteCompraNav()
        {
            Page page = new CompraPage();
            page.Title = "Compra";
            App.RootPage.Detail = new NavigationPage(page);
        }

        private void ExecuteVentaNav()
        {
            Page page = new VentaPage();
            page.Title = "Ventas";
            App.RootPage.Detail = new NavigationPage(page);
        }

        private void ExecuteProveedorNav()
        {
            Page page = new ProveedorPage();
            page.Title = "Proveedores";
            App.RootPage.Detail = new NavigationPage(page);
        }
        #endregion

        #region ========================= LOADS =========================
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
        #endregion
    }
    public class UltimasVentas
    {
        public string dia { get; set; }
        public dynamic idVenta { get; set; }
        public string total { get; set; }
    }
}
