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
    class OrdenCompraViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public OrdenCompraItemViewModel CurrentOrdenCompra { get; set; }

        // PERSONAL
        private Personal _PersonalSelectedItem;
        public Personal PersonalSelectedItem
        {
            get { return this._PersonalSelectedItem; }
            set { SetValue(ref this._PersonalSelectedItem, value); }
        }

        private List<Personal> _PersonalItems;
        public List<Personal> PersonalItems
        {
            get { return this._PersonalItems; }
            set { SetValue(ref this._PersonalItems, value); }
        }

        // SUCURSAL
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
        // --

        private List<OrdenCompra> OrdenCompraList { get; set; }
        private ObservableCollection<OrdenCompraItemViewModel> _OrdenCompraItems;
        public ObservableCollection<OrdenCompraItemViewModel> OrdenCompraItems
        {
            get { return this._OrdenCompraItems; }
            set { SetValue(ref this._OrdenCompraItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public OrdenCompraViewModel()
        {
            instance = this;
            this.CurrentOrdenCompra = new OrdenCompraItemViewModel();
            this.loadRoot();
        }

        public override void ExecuteRefresh()
        {
            this.OrdenCompraItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.OrdenCompraItems = new ObservableCollection<OrdenCompraItemViewModel>(
                    this.ToClienteItemViewModel());
            }
            else
            {
                this.OrdenCompraItems = new ObservableCollection<OrdenCompraItemViewModel>(
                    this.ToClienteItemViewModel().Where(
                        m => m.nombres.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            throw new NotImplementedException();
        }

        #region ===================================== LOADS =====================================
        private void loadRoot()
        {
            cargarSucursal();
            cargarPersonales();

            LoadRegisters();
        }

        private async void cargarSucursal()
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

                RootObject<OrdenCompra> rootData = await webService.GET<RootObject<OrdenCompra>>("ocompras", String.Format("suc/{0}/per/{1}/{2}/{3}", sucursalId, personalId, paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.OrdenCompraList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.OrdenCompraItems = new ObservableCollection<OrdenCompraItemViewModel>(
                    this.ToClienteItemViewModel());
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

        internal void SetCurrentOrdenCompra(OrdenCompraItemViewModel ordenCompraItemViewModel)
        {
            this.CurrentOrdenCompra = ordenCompraItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<OrdenCompraItemViewModel> ToClienteItemViewModel()
        {
            return OrdenCompraList.Select(o => new OrdenCompraItemViewModel
            {
                idOrdenCompra = o.idOrdenCompra,
                serie = o.serie,
                correlativo = o.correlativo,
                nombreProveedor = o.nombreProveedor,
                rucDni = o.rucDni,
                direccionProveedor = o.direccionProveedor,
                moneda = o.moneda,
                fecha = o.fecha,
                plazoEntrega = o.plazoEntrega,
                observacion = o.observacion,
                direccion = o.direccion,
                estado = o.estado,
                idUbicacionGeografica = o.idUbicacionGeografica,
                idTipoDocumento = o.idTipoDocumento,
                idCompra = o.idCompra,
                subTotal = o.subTotal,
                total = o.total,
                idPago = o.idPago,
                idProveedor = o.idProveedor,
                estadoCompra = o.estadoCompra,
                nombres = o.nombres,

                BackgroundItem = (o.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (o.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static OrdenCompraViewModel instance;

        public static OrdenCompraViewModel GetInstance()
        {
            if (instance == null)
            {
                return new OrdenCompraViewModel();
            }
            return instance;
        }
        #endregion
    }
}
