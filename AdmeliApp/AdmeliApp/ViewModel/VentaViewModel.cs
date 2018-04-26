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
    public class VentaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public VentaItemViewModel CurrentVenta { get; set; }

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

        // Lista principal
        private List<Venta> VentaList { get; set; }
        private ObservableCollection<VentaItemViewModel> _VentaItems;
        public ObservableCollection<VentaItemViewModel> VentaItems
        {
            get { return this._VentaItems; }
            set { SetValue(ref this._VentaItems, value); }
        }

        // Commands
        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        // Constructor
        public VentaViewModel()
        {
            instance = this;
            this.CurrentVenta = new VentaItemViewModel();
            this.loadRoot();
        }

        public override void ExecuteRefresh()
        {
            this.VentaItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.VentaItems = new ObservableCollection<VentaItemViewModel>(
                    this.ToVentaItemViewModel());
            }
            else
            {
                this.VentaItems = new ObservableCollection<VentaItemViewModel>(
                    this.ToVentaItemViewModel().Where(
                        m => m.nombreLabel.ToLower().Contains(this.SearchText.ToLower())));
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
            cargarPuntoVenta();
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

        private async void cargarPuntoVenta()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/puntoventasytodos/suc/0
                PuntoVentaItems = await webService.GET<List<PuntoVenta>>("puntoventasytodos", String.Format("suc/{0}", App.sucursal.idSucursal));
                PuntoVentaSelectedItem = PuntoVentaItems.Find(p => p.idPuntoVenta == 0);
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
                int puntoVentaId = (PuntoVentaSelectedItem == null) ? 1 : PuntoVentaSelectedItem.idPuntoVenta;

                // www.lineatienda.com/services.php/ventas/sucursal/0/puntoventa/0/per/0/estado/todos/1/100
                RootObject<Venta> rootData = await webService.GET<RootObject<Venta>>("ventas", String.Format("sucursal/{0}/puntoventa/{1}/per/{2}/estado/{3}/{4}/{5}", sucursalId, puntoVentaId, personalId, estado, paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.VentaList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.VentaItems = new ObservableCollection<VentaItemViewModel>(
                    this.ToVentaItemViewModel());
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

        internal void SetCurrentVenta(VentaItemViewModel ventaItemViewModel)
        {
            this.CurrentVenta = ventaItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<VentaItemViewModel> ToVentaItemViewModel()
        {
            return VentaList.Select(m => new VentaItemViewModel
            {
                idVenta = m.idVenta,
                fecha = m.fecha,
                serie = m.serie,
                correlativo = m.correlativo,
                nombreCliente = m.nombreCliente,
                rucDni = m.rucDni,
                direccion = m.direccion,
                idGrupoCliente = m.idGrupoCliente,
                formaPago = m.formaPago,
                fechaPago = m.fechaPago,
                fechaVenta = m.fechaVenta,
                observacion = m.observacion,
                subTotal = m.subTotal,
                total = m.total,
                descuento = m.descuento,
                tipoVenta = m.tipoVenta,
                moneda = m.moneda,
                tipoCambio = m.tipoCambio,
                estado = m.estado,
                idCliente = m.idCliente,
                idCobro = m.idCobro,
                idAsignarPuntoVenta = m.idAsignarPuntoVenta,
                idTipoDocumento = m.idTipoDocumento,
                nombreLabel = m.nombreLabel,
                notaSalida = m.notaSalida,
                vendedor = m.vendedor,
                guiaRemision = m.guiaRemision,
                documentoIdentificacion = m.documentoIdentificacion,
                numeroDocumento = m.numeroDocumento,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static VentaViewModel instance;

        public static VentaViewModel GetInstance()
        {
            if (instance == null)
            {
                return new VentaViewModel();
            }
            return instance;
        }
        #endregion
    }
    public class EstadoVenta
    {
        public string idEstado { get; set; }
        public string nombre { get; set; }
    }
}
