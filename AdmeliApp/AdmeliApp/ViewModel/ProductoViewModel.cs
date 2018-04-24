using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
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
    public class ProductoViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public ProductoItemViewModel CurrentProducto { get; set; }

        private List<Producto> ProductoList { get; set; }
        private ObservableCollection<ProductoItemViewModel> _ProductoItems;
        public ObservableCollection<ProductoItemViewModel> ProductoItems
        {
            get { return this._ProductoItems; }
            set { SetValue(ref this._ProductoItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        #region ============================== CONSTRUCTOR ==============================
        public ProductoViewModel()
        {
            instance = this;
            this.CurrentProducto = new ProductoItemViewModel();
            this.LoadRegisters();
        } 
        #endregion

        #region =============================== EXECUTES ===============================
        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.ProductoItems = new ObservableCollection<ProductoItemViewModel>(
                    this.ToProductoItemViewModel());
            }
            else
            {
                this.ProductoItems = new ObservableCollection<ProductoItemViewModel>(
                    this.ToProductoItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentProducto(new ProductoItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.ProductosPage.Navigation.PushAsync(new ProductoItemPage());
        }
        #endregion

        #region ================================ LOADS ================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/productos/categoria/1/100
                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);

                Dictionary<string, int>[] dataSend = { list };

                RootObject<Producto> rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto>>("productos", String.Format("categoria/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina), dataSend);
                this.ProductoList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.ProductoItems = new ObservableCollection<ProductoItemViewModel>(
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
        internal void SetCurrentProducto(ProductoItemViewModel productoItemViewModel)
        {
            this.CurrentProducto = productoItemViewModel;
        }

        private IEnumerable<ProductoItemViewModel> ToProductoItemViewModel()
        {
            return ProductoList.Select(p => new ProductoItemViewModel
            {

                idProducto = p.idProducto,
                cantidadFraccion = p.cantidadFraccion,
                codigoBarras = p.codigoBarras,
                codigoProducto = p.codigoProducto,
                controlSinStock = p.controlSinStock,
                descripcionCorta = p.descripcionCorta,
                descripcionLarga = p.descripcionLarga,
                enCategoriaEstrella = p.enCategoriaEstrella,
                enPortada = p.enPortada,
                enUso = p.enUso,
                estado = p.estado,
                idMarca = p.idMarca,
                idUnidadMedida = p.idUnidadMedida,
                keywords = p.keywords,
                limiteMaximo = p.limiteMaximo,
                limiteMinimo = p.limiteMinimo,
                mostrarPrecioWeb = p.mostrarPrecioWeb,
                mostrarVideo = p.mostrarVideo,
                mostrarWeb = p.mostrarWeb,
                nombreMarca = p.nombreMarca,
                nombreProducto = p.nombreProducto,
                nombreUnidad = p.nombreUnidad,
                precioCompra = p.precioCompra,
                urlVideo = p.urlVideo,
                ventaVarianteSinStock = p.ventaVarianteSinStock,
                nombre = p.nombre,
                codigo = p.codigo,

                //BackgroundItem = (p.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                //TextColorItem = (p.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static ProductoViewModel instance;

        public static ProductoViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductoViewModel();
            }
            return instance;
        }
        #endregion
    }
}
