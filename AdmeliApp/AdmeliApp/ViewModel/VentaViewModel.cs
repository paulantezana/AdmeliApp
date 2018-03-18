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

        private List<Venta> VentaList { get; set; }
        private ObservableCollection<VentaItemViewModel> _VentaItems;
        public ObservableCollection<VentaItemViewModel> VentaItems
        {
            get { return this._VentaItems; }
            set { SetValue(ref this._VentaItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public VentaViewModel()
        {
            instance = this;
            this.CurrentVenta = new VentaItemViewModel();
            this.LoadRegisters();
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
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/almacenes/estado/1/100
                RootObject<Venta> rootData = await webService.GET<RootObject<Venta>>("almacenes", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
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

        internal void SetCurrentMarca(VentaItemViewModel ventaItemViewModel)
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
}
