using AdmeliApp.Helpers;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class ConfigInitialViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        // Almacen
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

        // PUNETO VENTA
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


        public ConfigInitialViewModel()
        {
            this.IsEnabled = true;
            PuntoVentaItems = App.puntosDeVenta;
            AlmacenItems = App.alamacenes;
        }

        // COMMANDS
        private ICommand _ContinuarCommand;
        public ICommand ContinuarCommand =>
            _ContinuarCommand ?? (_ContinuarCommand = new Command(() => ExecuteContinuar()));

        private void ExecuteContinuar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if(PuntoVentaSelectedItem != null || AlmacenSelectedItem != null)
            {
                // Estableciendo los IDAlmacen y IDPunto Venta por defecto
                App.currentIdAlmacen = AlmacenSelectedItem.idAlmacen;
                App.currentIdPuntoVenta = PuntoVentaSelectedItem.idAsignarPuntoVenta;

                // Mostrando la pagina principal
                App.Current.MainPage = new AdmeliApp.Pages.Root.RootPage();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error","No soleciono ninguno", "Aceptar");
            }
            this.IsEnabled = true;
            this.IsRunning = false;
        }
    }
}
