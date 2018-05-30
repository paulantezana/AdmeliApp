using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using AdmeliApp.Pages.ProductoPages.Util;
using AdmeliApp.Pages.VentaPages.VentaItemPages;
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
    class VentaTouchViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------
        private List<ProductoVenta> ProductoVentaList { get; set; }
        private ObservableCollection<ProductoVentaItemViewModel> _ProductoVentaItems;
        public ObservableCollection<ProductoVentaItemViewModel> ProductoVentaItems
        {
            get { return this._ProductoVentaItems; }
            set { SetValue(ref this._ProductoVentaItems, value); }
        }

        private ICommand _ViewCartCommand;
        public ICommand ViewCartCommand =>
            _ViewCartCommand ?? (_ViewCartCommand = new Command(() => ExecuteViewCart()));

        #region ============================== CONSTRUCTOR ==============================
        public VentaTouchViewModel()
        {
            instance = this;
            this.loadRoot();
        }
        #endregion

        #region =============================== EXECUTES ===============================
        public override void ExecuteSearch()
        {
            LoadSerach();
        }

        public override void ExecuteRefresh()
        {
            if (SearchText != string.Empty)
            {
                LoadSerach();
            }
            else
            {
                LoadRegisters();
            }
        }

        public override void ExecuteSearchRealTime()
        {
            if (SearchText == string.Empty) this.loadRoot();
        }

        private void ExecuteViewCart()
        {
            //this.SetCurrentProducto(new ViewCartViewModel());
            App.VentaTouchPage.Navigation.PushAsync(new ViewCartPage()); // Navegacion
        }
        #endregion

        #region ================================ LOADS ================================
        private void loadRoot()
        {
            LoadRegisters();
        }

        public void LoadSerach()
        {
            LoadRegisters();
        }

        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                if (SearchText == null) SearchText = string.Empty;

                // localhost/admeli/xcore/services.php/productos/suc/1/personal/1
                ProductoVentaList = await webService.GET<List<ProductoVenta>>("productos", String.Format("suc/{0}/personal/{1}", App.sucursal.idSucursal, App.personal.idPersonal));

                // Set data paginacion
                this.paginacion.itemsCount = ProductoVentaList.Count;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.ProductoVentaItems = new ObservableCollection<ProductoVentaItemViewModel>(
                    this.ToProductoItemViewModel());
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
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<ProductoVentaItemViewModel> ToProductoItemViewModel()
        {
            return ProductoVentaList.Select(p => new ProductoVentaItemViewModel
            {
                idProducto = p.idProducto,
                codigoProducto = p.codigoProducto,
                nombreProducto = p.nombreProducto,
                precioVenta = p.precioVenta,
                idPresentacion = p.idPresentacion,
                ventaVarianteSinStock = p.ventaVarianteSinStock,
                nombreMarca = p.nombreMarca,

                IsVisiblePrecioVenta = (p.precioVenta == null)
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static VentaTouchViewModel instance;

        public static VentaTouchViewModel GetInstance()
        {
            if (instance == null)
            {
                return new VentaTouchViewModel();
            }
            return instance;
        }
        #endregion
    }
}
