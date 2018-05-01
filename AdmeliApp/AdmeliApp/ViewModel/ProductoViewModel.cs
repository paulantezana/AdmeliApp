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

        // PERSONAL
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

        // --------------------------------------
        private bool _IsEnabledStock;
        public bool IsEnabledStock
        {
            get { return this._IsEnabledStock; }
            set { SetValue(ref this._IsEnabledStock, value); }
        }

        private List<Producto> ProductoList { get; set; }
        private ObservableCollection<ProductoItemViewModel> _ProductoItems;
        public ObservableCollection<ProductoItemViewModel> ProductoItems
        {
            get { return this._ProductoItems; }
            set { SetValue(ref this._ProductoItems, value); }
        }

        #region ============================== CONSTRUCTOR ==============================
        public ProductoViewModel()
        {
            instance = this;
            this.CurrentProducto = new ProductoItemViewModel();
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
        #endregion

        #region ================================ LOADS ================================
        private void loadRoot()
        {
            cargarSucursal();
            cargarAlmacen();

            LoadRegisters();
        }

        private async void cargarAlmacen()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/almacenes/id/nombre/estado/1
                int estado = 1;
                AlmacenItems = await webService.GET<List<Almacen>>("almacenes", String.Format("id/nombre/estado/{0}", estado));
                AlmacenSelectedItem = AlmacenItems.Find(a => a.idAlmacen == 0);
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

        public async void LoadSerach()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);

                Dictionary<string, int>[] dataSend = { list };

                RootObject<Producto> rootData;
                if (IsEnabledStock)
                {
                    
                    // www.lineatienda.com/services.php/productos/categoria/stock/1/100/1/1
                    int almacenId = (AlmacenSelectedItem == null) ? 1 : AlmacenSelectedItem.idAlmacen;
                    int sucursalId = (SucursalSelectedItem == null) ? App.sucursal.idSucursal : SucursalSelectedItem.idSucursal;

                    rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto>>("productos", String.Format("categoria/stock/{0}/{1}/{2}/{3}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina, almacenId, sucursalId), dataSend);
                }
                else
                {
                    // www.lineatienda.com/services.php/productos/categoria/1/100
                    rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto>>("productos", String.Format("categoria/{0}/{1}/{2}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina, SearchText), dataSend);
                }

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
