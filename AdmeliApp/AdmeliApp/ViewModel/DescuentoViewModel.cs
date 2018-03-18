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
    public class DescuentoViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public DescuentoItemViewModel CurrentDescuento { get; set; }

        private List<Descuento> DescuentoList { get; set; }
        private ObservableCollection<DescuentoItemViewModel> _DescuentoItems;
        public ObservableCollection<DescuentoItemViewModel> DescuentoItems
        {
            get { return this._DescuentoItems; }
            set { SetValue(ref this._DescuentoItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public DescuentoViewModel()
        {
            instance = this;
            this.CurrentDescuento = new DescuentoItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.DescuentoItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.DescuentoItems = new ObservableCollection<DescuentoItemViewModel>(
                    this.ToDescuentoItemViewModel());
            }
            else
            {
                this.DescuentoItems = new ObservableCollection<DescuentoItemViewModel>(
                    this.ToDescuentoItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
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
                RootObject<Descuento> rootData = await webService.GET<RootObject<Descuento>>("almacenes", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.DescuentoList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.DescuentoItems = new ObservableCollection<DescuentoItemViewModel>(
                    this.ToDescuentoItemViewModel());
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

        internal void SetCurrentMarca(DescuentoItemViewModel descuentoItemViewModel)
        {
            this.CurrentDescuento = descuentoItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<DescuentoItemViewModel> ToDescuentoItemViewModel()
        {
            return DescuentoList.Select(m => new DescuentoItemViewModel
            {
                idDescuentoProductoGrupo = m.idDescuentoProductoGrupo,
                codigo = m.codigo,
                descuento = m.descuento,
                fechaInicio = m.fechaInicio,
                fechaFin = m.fechaFin,
                tipoDescuento = m.tipoDescuento,
                tipo = m.tipo,
                cantidadMinima = m.cantidadMinima,
                cantidadMaxima = m.cantidadMaxima,
                estado = m.estado,
                idGrupoCliente = m.idGrupoCliente,
                nombreGrupo = m.nombreGrupo,
                idSucursal = m.idSucursal,
                nombre = m.nombre,
                idAfectoProducto = m.idAfectoProducto,
                idProducto = m.idProducto,
                nombreSucursal = m.nombreSucursal,
                nombreProducto = m.nombreProducto,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static DescuentoViewModel instance;

        public static DescuentoViewModel GetInstance()
        {
            if (instance == null)
            {
                return new DescuentoViewModel();
            }
            return instance;
        }
        #endregion
    }
}
