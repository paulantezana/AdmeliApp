using AdmeliApp.Helpers;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class ConfigInitialViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        // Almacen
        private bool _AlmacenIsVisible;
        public bool AlmacenIsVisible
        {
            get { return this._AlmacenIsVisible; }
            set { SetValue(ref this._AlmacenIsVisible, value); }
        }

        private Almacen _AlmacenSelectedItem;
        public Almacen AlmacenSelectedItem
        {
            get { return this._AlmacenSelectedItem; }
            set { SetValue(ref this._AlmacenSelectedItem, value); }
        }

        private List<Almacen> _AlmacenItems;
        public List<Almacen> AlmacenItems
        {
            get { return this._AlmacenItems; }
            set { SetValue(ref this._AlmacenItems, value); }
        }


        // UNTO DE VENTA
        private bool _PuntoVentaIsVisible;
        public bool PuntoVentaIsVisible
        {
            get { return this._PuntoVentaIsVisible; }
            set { SetValue(ref this._PuntoVentaIsVisible, value); }
        }

        private PuntoVenta _PuntoVentaSelectedItem;
        public PuntoVenta PuntoVentaSelectedItem
        {
            get { return this._PuntoVentaSelectedItem; }
            set { SetValue(ref this._PuntoVentaSelectedItem, value); }
        }

        private List<PuntoVenta> _PuntoVentaItems;
        public List<PuntoVenta> PuntoVentaItems
        {
            get { return this._PuntoVentaItems; }
            set { SetValue(ref this._PuntoVentaItems, value); }
        }


        public  ConfigInitialViewModel()
        {
            this.IsEnabled = true;

            PuntoVentaItems = App.puntosDeVenta;
            AlmacenItems = App.almacenes;

            if(PuntoVentaItems.Count > 0)
            {
                PuntoVentaIsVisible = true;
                DefaultPuntoVenta();
            }

            if (AlmacenItems.Count > 0)
            {
                AlmacenIsVisible = true;
                DafaultAlmacen();
            }
            
        }

        // COMMANDS
        private ICommand _ContinuarCommand;
        public ICommand ContinuarCommand =>
            _ContinuarCommand ?? (_ContinuarCommand = new Command(() => ExecuteContinuar()));

        #region ====================================== DAFAULT ======================================
        private async void DafaultAlmacen()
        {
            await Task.Run(() => Thread.Sleep(10)); // Es una solucion no tan eficiente si se encuentra otra forma de seleccionar un item por defecto -- bienvenido
            AlmacenSelectedItem = AlmacenItems[0];
        }

        private async void DefaultPuntoVenta()
        {
            await Task.Run(() => Thread.Sleep(10)); // Es una solucion no tan eficiente si se encuentra otra forma de seleccionar un item por defecto -- bienvenido
            PuntoVentaSelectedItem = PuntoVentaItems[0];
        } 
        #endregion

        private async void ExecuteContinuar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;

            if (PuntoVentaSelectedItem == null && PuntoVentaIsVisible)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "No selecciono un punto de venta", "Aceptar");
                return;
            }

            if (AlmacenSelectedItem == null && AlmacenIsVisible)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "No selecciono un almacen", "Aceptar");
                return;
            }

            // Estableciendo los IDAlmacen y IDPunto Venta por defecto
            if (AlmacenSelectedItem != null && AlmacenIsVisible) App.currentIdAlmacen = AlmacenSelectedItem.idAlmacen;
            if (PuntoVentaSelectedItem != null && AlmacenIsVisible) App.currentIdPuntoVenta = PuntoVentaSelectedItem.idAsignarPuntoVenta;

            // Mostrando el panel principal de la APP
            App.Current.MainPage = new AdmeliApp.Pages.Root.RootPage();

            this.IsEnabled = true;
            this.IsRunning = false;
        }
    }
}
