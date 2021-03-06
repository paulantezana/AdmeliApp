﻿using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using AdmeliApp.Pages.ProductoPages.Util;
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

        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------
        private bool _IsEnabledStock;
        public bool IsEnabledStock
        {
            get { return this._IsEnabledStock; }
            set {
                SetValue(ref this._IsEnabledStock, value);
                this.LoadRegisters();
            }
        }

        private List<Producto> ProductoList { get; set; }
        private ObservableCollection<ProductoItemViewModel> _ProductoItems;
        public ObservableCollection<ProductoItemViewModel> ProductoItems
        {
            get { return this._ProductoItems; }
            set { SetValue(ref this._ProductoItems, value); }
        }

        // COMANDS
        private ICommand _FilterCommand;
        public ICommand FilterCommand =>
            _FilterCommand ?? (_FilterCommand = new Command(() => ExecuteFilterAsync()));

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        #region ============================== CONSTRUCTOR ==============================
        public ProductoViewModel()
        {
            instance = this;
            IsEnabledStock = true;

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

        private void ExecuteFilterAsync()
        {
            // this.SetCurrentMarca(new MarcaItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.ProductosPage.Navigation.PushAsync(new SelectableCategoriaPage()); // Navegacion
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentProducto(new ProductoItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.ProductosPage.Navigation.PushAsync(new ProductoItemPage()); // Navegacion
        }
        #endregion

        #region ================================ LOADS ================================
        private void loadRoot()
        {
            //cargarSucursal();
            //cargarAlmacen();

            LoadRegisters();
        }

        //private async void cargarAlmacen()
        //{
        //    try
        //    {
        //        this.IsRefreshing = true;
        //        this.IsEnabled = false;

        //        // www.lineatienda.com/services.php/almacenes/id/nombre/estado/1
        //        int estado = 1;
        //        AlmacenItems = await webService.GET<List<Almacen>>("almacenes", String.Format("id/nombre/estado/{0}", estado));
        //        AlmacenSelectedItem = AlmacenItems.Find(a => a.idAlmacen == 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        //    }
        //    finally
        //    {
        //        this.IsRefreshing = false;
        //        this.IsEnabled = true;
        //    }
        //}

        //private async void cargarSucursal()
        //{
        //    try
        //    {
        //        this.IsRefreshing = true;
        //        this.IsEnabled = false;

        //        // www.lineatienda.com/services.php/listarsucursalesactivos
        //        SucursalItems = await webService.GET<List<Sucursal>>("listarsucursalesactivos");
        //        SucursalSelectedItem = SucursalItems.Find(s => s.idSucursal == App.sucursal.idSucursal);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        //    }
        //    finally
        //    {
        //        this.IsRefreshing = false;
        //        this.IsEnabled = true;
        //    }
        //}

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

                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);

                // Preparando datos
                Dictionary<string, int>[] dataSend = { list };

                //int almacenId = App.currentIdAlmacen;
                int almacenId = 0;
                int sucursalId = 0;

                RootObject<Producto, CombinacionStock> rootData;  // Variable donde se almacenara la respuesta de la API
                if (SearchText == null) SearchText = string.Empty;

                if (SearchText != string.Empty) // Cuando el cuadro de busueda esta bacia
                {
                    if (IsEnabledStock) // Cuando el stock sta havilitado
                    {
                        // localhost:8080/admeli/xcore/services.php/productos/categoria/stock/1/30/iphone/0/0
                        rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto, CombinacionStock>>("productos", String.Format("categoria/stock/{0}/{1}/{2}/{3}/{4}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina, SearchText, almacenId, sucursalId), dataSend);
                    }
                    else
                    {
                        // localhost:8080/admeli/xcore/services.php/productos/categoria/1/10/monitor
                        rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto, CombinacionStock>>("productos", String.Format("categoria/{0}/{1}/{2}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina, SearchText), dataSend);
                    }
                }

                else
                {
                    if (IsEnabledStock) // Cuando el stock sta havilitado
                    {
                        // localhost:8080/admeli/xcore/services.php/productos/categoria/stock/1/10/1/1
                        rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto, CombinacionStock>>("productos", String.Format("categoria/stock/{0}/{1}/{2}/{3}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina, almacenId, sucursalId), dataSend);
                    }
                    else
                    {
                        // localhost:8080/admeli/xcore/services.php/productos/categoria/1/10
                        rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto, CombinacionStock>>("productos", String.Format("categoria/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina), dataSend);
                    }
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
                idPresentacion = p.idPresentacion,
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
                EstadoString = p.EstadoString,
                precioVenta = p.precioVenta,
                stock = p.stock,
                stockFinanciero = p.stockFinanciero,
                idPresentacionAfectada = p.idPresentacionAfectada,
                idAlmacen = p.idAlmacen,
                nombreAlmacen = p.nombreAlmacen,
                IdPresentacionint = p.IdPresentacionint,

                IsVisiblePrecioVenta = (p.precioVenta != null),
                //IsVisiblePrecioCompra = (p.precioCompra != null),
                IsVisibleStock = (p.stock != null),

                BackgroundItem = (!p.estado) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (!p.estado) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["grey"],
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

