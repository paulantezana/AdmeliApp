using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class CotizacionViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public CotizacionItemViewModel CurrentCotizacion { get; set; }

        private Personal _PersonalSelectedItem;
        public Personal PersonalSelectedItem
        {
            get { return this._PersonalSelectedItem; }
            set { SetValue(ref this._PersonalSelectedItem, value); }
        }

        private Sucursal _SucursalSelectedItem;
        public Sucursal SucursalSelectedItem
        {
            get { return this._SucursalSelectedItem; }
            set { SetValue(ref this._SucursalSelectedItem, value); }
        }

        private List<Sucursal> _SucursalItems;
        public List<Sucursal> SucursalItems
        {
            get { return this._SucursalItems; }
            set { SetValue(ref this._SucursalItems, value); }
        }

        private List<Personal> _PersonalItems;
        public List<Personal> PersonalItems
        {
            get { return this._PersonalItems; }
            set { SetValue(ref this._PersonalItems, value); }
        }

        private List<Cotizacion> CotizacionList { get; set; }
        private ObservableCollection<CotizacionItemViewModel> _CotizacionItems;
        public ObservableCollection<CotizacionItemViewModel> CotizacionItems
        {
            get { return this._CotizacionItems; }
            set { SetValue(ref this._CotizacionItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public CotizacionViewModel()
        {
            instance = this;
            this.CurrentCotizacion = new CotizacionItemViewModel();
            this.loadRoot(); // Load registers
        }

        public override void ExecuteRefresh()
        {
            this.CotizacionItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.CotizacionItems = new ObservableCollection<CotizacionItemViewModel>(
                    this.ToCotizacionItemViewModel());
            }
            else
            {
                this.CotizacionItems = new ObservableCollection<CotizacionItemViewModel>(
                    this.ToCotizacionItemViewModel().Where(
                        m => m.nombreCliente.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            throw new NotImplementedException();
        }

        #region ===================================== LOADS =====================================
        private void loadRoot()
        {
            cargarSucursales();
            cargarPersonales();
            LoadRegisters();
        }

        private async void cargarSucursales()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/listarsucursalesactivos
                SucursalItems = await webService.GET<List<Sucursal>>("listarsucursalesactivos");
                SucursalSelectedItem = SucursalItems.Find(s => s.idSucursal == 0);
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

        private async void cargarPersonales()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/listarpersonalalmacen/sucursal/0
                PersonalItems = await webService.GET<List<Personal>>("listarpersonalalmacen", String.Format("sucursal/{0}", App.sucursal.idSucursal));
                PersonalSelectedItem = PersonalItems.Find(p => p.idPersonal == 0);
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

        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                int personalId = (PersonalSelectedItem == null) ? App.personal.idPersonal : PersonalSelectedItem.idPersonal;
                int sucursalId = (SucursalSelectedItem == null) ? App.sucursal.idSucursal : SucursalSelectedItem.idSucursal;

                // www.lineatienda.com/services.php/cotizaciones/suc/0/per/0/1/100
                RootObject<Cotizacion> rootData = await webService.GET<RootObject<Cotizacion>>("cotizaciones", String.Format("suc/{0}/per/{1}/{2}/{3}", sucursalId, personalId, paginacion.currentPage, App.configuracionGeneral.itemPorPagina));

                this.CotizacionList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.CotizacionItems = new ObservableCollection<CotizacionItemViewModel>(
                    this.ToCotizacionItemViewModel());
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

        internal void SetCurrentCotizacion(CotizacionItemViewModel cotizacionItemViewModel)
        {
            this.CurrentCotizacion = cotizacionItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<CotizacionItemViewModel> ToCotizacionItemViewModel()
        {
            return CotizacionList.Select(m => new CotizacionItemViewModel
            {
                idCotizacion = m.idCotizacion,
                serie = m.serie,
                correlativo = m.correlativo,
                nombreCliente = m.nombreCliente,
                direccion = m.direccion,
                rucDni = m.rucDni,
                idGrupoCliente = m.idGrupoCliente,
                moneda = m.moneda,
                descuento = m.descuento,
                subTotal = m.subTotal,
                total = m.total,
                tipoCambio = m.tipoCambio,
                idMoneda = m.idMoneda,
                fechaEmision = m.fechaEmision,
                fechaVencimiento = m.fechaVencimiento,
                observacion = m.observacion,
                estado = m.estado,
                idCliente = m.idCliente,
                idSucursal = m.idSucursal,
                personal = m.personal,
                documentoIdentificacion = m.documentoIdentificacion,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static CotizacionViewModel instance;

        public static CotizacionViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CotizacionViewModel();
            }
            return instance;
        }
        #endregion
    }
}
