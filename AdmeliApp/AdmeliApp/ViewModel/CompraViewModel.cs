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
    public class CompraViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public CompraItemViewModel CurrentCompra { get; set; }

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

        // ESTADO VENTA
        private int _EstadoSelectedIndex;
        public int EstadoSelectedIndex
        {
            get { return this._EstadoSelectedIndex; }
            set { SetValue(ref this._EstadoSelectedIndex, value); }
        }

        private EstadoVenta _EstadoVentaSelectedItem;
        public EstadoVenta EstadoVentaSelectedItem
        {
            get { return this._EstadoVentaSelectedItem; }
            set { SetValue(ref this._EstadoVentaSelectedItem, value); }
        }

        private List<EstadoVenta> _EstadolItems;
        public List<EstadoVenta> EstadolItems
        {
            get { return this._EstadolItems; }
            set { SetValue(ref this._EstadolItems, value); }
        }
        // --

        private List<Compra> CompraList { get; set; }
        private ObservableCollection<CompraItemViewModel> _CompraItems;
        public ObservableCollection<CompraItemViewModel> CompraItems
        {
            get { return this._CompraItems; }
            set { SetValue(ref this._CompraItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public CompraViewModel()
        {
            instance = this;
            this.CurrentCompra = new CompraItemViewModel();
            this.loadRoot();
        }

        public override void ExecuteRefresh()
        {
            this.CompraItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.CompraItems = new ObservableCollection<CompraItemViewModel>(
                    this.ToCompraItemViewModel());
            }
            else
            {
                this.CompraItems = new ObservableCollection<CompraItemViewModel>(
                    this.ToCompraItemViewModel().Where(
                        m => m.nroOrdenCompra.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            //this.SetCurrentCompra(new CompraItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            //App.CompraPage.Navigation.PushAsync(new CompraItemPage());
        }

        #region ===================================== LOADS =====================================
        private void loadRoot()
        {
            cargarSucursal();
            cargarPersonales();
            cargarEstados();

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
                EstadoSelectedIndex = 1;        // Estado seleccionado por defecto
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

        private void cargarEstados()
        {
            EstadolItems = new List<EstadoVenta>()
            {
                new EstadoVenta()
                {
                    idEstado = "1",
                    nombre = "Activos"
                },

                new EstadoVenta()
                {
                    idEstado = "todos",
                    nombre = "Todos los estados"
                },

                new EstadoVenta()
                {
                    idEstado = "0",
                    nombre = "Anulados"
                },
            };
        }
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                int personalId = (PersonalSelectedItem == null) ? App.personal.idPersonal : PersonalSelectedItem.idPersonal;
                int sucursalId = (SucursalSelectedItem == null) ? App.sucursal.idSucursal : SucursalSelectedItem.idSucursal;
                string estado = (EstadoVentaSelectedItem == null) ? "todos" : EstadoVentaSelectedItem.idEstado;

                RootObject<Compra> rootData = await webService.GET<RootObject<Compra>>("compras", String.Format("sucursal/{0}/personal/{1}/estado/{2}/{3}/{4}", sucursalId, personalId, estado, paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.CompraList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.CompraItems = new ObservableCollection<CompraItemViewModel>(
                    this.ToCompraItemViewModel());
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

        internal void SetCurrentCompra(CompraItemViewModel compraItemViewModel)
        {
            this.CurrentCompra = compraItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<CompraItemViewModel> ToCompraItemViewModel()
        {
            return CompraList.Select(c => new CompraItemViewModel
            {
                idCompra = c.idCompra,
                numeroDocumento = c.numeroDocumento,
                nroOrdenCompra = c.nroOrdenCompra,
                nombreProveedor = c.nombreProveedor,
                rucDni = c.rucDni,
                direccion = c.direccion,
                formaPago = c.formaPago,
                moneda = c.moneda,
                fechaFacturacion = c.fechaFacturacion,
                fechaPago = c.fechaPago,
                descuento = c.descuento,
                tipoCompra = c.tipoCompra,
                tipoCambio = c.tipoCambio,
                subTotal = c.subTotal,
                total = c.total,
                observacion = c.observacion,
                estado = c.estado,
                idProveedor = c.idProveedor,
                idPago = c.idPago,
                idPersonal = c.idPersonal,
                idTipoDocumento = c.idTipoDocumento,
                nombreLabel = c.nombreLabel,
                idSucursal = c.idSucursal,
                vendedor = c.vendedor,
                idCajaSesion = c.idCajaSesion,
                fecha = c.fecha,

                BackgroundItem = (c.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (c.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static CompraViewModel instance;

        public static CompraViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CompraViewModel();
            }
            return instance;
        }
        #endregion
    }
}
