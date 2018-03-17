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
    public class PuntoVentaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public PuntoVentaItemViewModel CurrentPuntoVenta { get; set; }

        private List<PuntoVenta> PuntoVentaList { get; set; }
        private ObservableCollection<PuntoVentaItemViewModel> _PuntoVentaItems;
        public ObservableCollection<PuntoVentaItemViewModel> PuntoVentaItems
        {
            get { return this._PuntoVentaItems; }
            set { SetValue(ref this._PuntoVentaItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public PuntoVentaViewModel()
        {
            instance = this;
            this.CurrentPuntoVenta = new PuntoVentaItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.PuntoVentaItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.PuntoVentaItems = new ObservableCollection<PuntoVentaItemViewModel>(
                    this.ToPuntoVentaItemViewModel());
            }
            else
            {
                this.PuntoVentaItems = new ObservableCollection<PuntoVentaItemViewModel>(
                    this.ToPuntoVentaItemViewModel().Where(
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

                // www.lineatienda.com/services.php/marcas/estado/1/100
                RootObject<PuntoVenta> rootData = await webService.GET<RootObject<PuntoVenta>>("marcas", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.PuntoVentaList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.PuntoVentaItems = new ObservableCollection<PuntoVentaItemViewModel>(
                    this.ToPuntoVentaItemViewModel());
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

        internal void SetCurrentMarca(PuntoVentaItemViewModel puntoVentaItemViewModel)
        {
            this.CurrentPuntoVenta = puntoVentaItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<PuntoVentaItemViewModel> ToPuntoVentaItemViewModel()
        {
            return PuntoVentaList.Select(m => new PuntoVentaItemViewModel
            {
                idPuntoVenta = m.idPuntoVenta,
                nombre = m.nombre,
                ventaWeb = m.ventaWeb,
                estado = m.estado,
                idSucursal = m.idSucursal,
                sucursal = m.sucursal,
                idAsignarPuntoVenta = m.idAsignarPuntoVenta,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static PuntoVentaViewModel instance;

        public static PuntoVentaViewModel GetInstance()
        {
            if (instance == null)
            {
                return new PuntoVentaViewModel();
            }
            return instance;
        }
        #endregion
    }
}
