﻿using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ConfiguracionPages.ConfiguracionItemPages;
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

        private EstadoPuntoVenta _EstadoSelectedItem;
        public EstadoPuntoVenta EstadoSelectedItem
        {
            get { return this._EstadoSelectedItem; }
            set { SetValue(ref this._EstadoSelectedItem, value); }
        }

        private int _EstadoSelectedIndex;
        public int EstadoSelectedIndex
        {
            get { return this._EstadoSelectedIndex; }
            set { SetValue(ref this._EstadoSelectedIndex, value); }
        }

        private Sucursal _SucursalSelectedItem;
        public Sucursal SucursalSelectedItem
        {
            get { return this._SucursalSelectedItem; }
            set { SetValue(ref this._SucursalSelectedItem, value); }
        }

        private int _SucursalSelectedIndex;
        public int SucursalSelectedIndex
        {
            get { return this._SucursalSelectedIndex; }
            set { SetValue(ref this._SucursalSelectedIndex, value); }
        }

        private List<Sucursal> _SucursalItems;
        public List<Sucursal> SucursalItems
        {
            get { return this._SucursalItems; }
            set { SetValue(ref this._SucursalItems, value); }
        }

        private List<EstadoPuntoVenta> _EstadolItems;
        public List<EstadoPuntoVenta> EstadolItems
        {
            get { return this._EstadolItems; }
            set { SetValue(ref this._EstadolItems, value); }
        }


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
            this.cargarSucursales();
            this.cargarEstados();
        }

        #region =============================== COMMAND EXECUTE ===============================
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
                    this.TiPuntoVentaItemViewModel());
            }
            else
            {
                this.PuntoVentaItems = new ObservableCollection<PuntoVentaItemViewModel>(
                    this.TiPuntoVentaItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentPuntoVenta(new PuntoVentaItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.PuntoVentaPage.Navigation.PushAsync(new PuntoVentaItemPage());
        } 
        #endregion

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                int idSucursal = (SucursalSelectedItem != null) ? Convert.ToInt32(SucursalSelectedItem.idSucursal) : App.sucursal.idSucursal;
                string idEstado = (EstadoSelectedItem != null) ? EstadoSelectedItem.idEstado : "todos";

                // www.lineatienda.com/services.php/puntoventas/sucursal/0/estado/todos/1/100
                RootObject<PuntoVenta> rootData = await webService.GET<RootObject<PuntoVenta>>("puntoventas", String.Format("sucursal/{0}/estado/{1}/{2}/{3}", idSucursal, idEstado, paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.PuntoVentaList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.PuntoVentaItems = new ObservableCollection<PuntoVentaItemViewModel>(
                    this.TiPuntoVentaItemViewModel());
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

        private async void cargarSucursales()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/listarsucursalesactivos
                SucursalItems = await webService.GET<List<Sucursal>>("listarsucursalesactivos");
                SucursalSelectedItem = SucursalItems.Find(s => s.idSucursal == App.sucursal.idSucursal);
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
            EstadolItems = new List<EstadoPuntoVenta>()
            {
                new EstadoPuntoVenta()
                {
                    idEstado = "1",
                    nombre = "Activos"
                },

                new EstadoPuntoVenta()
                {
                    idEstado = "todos",
                    nombre = "Todos los estados"
                },

                new EstadoPuntoVenta()
                {
                    idEstado = "0",
                    nombre = "Anulados"
                },
            };
        }

        internal void SetCurrentPuntoVenta(PuntoVentaItemViewModel puntoVentaItemViewModel)
        {
            this.CurrentPuntoVenta = puntoVentaItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<PuntoVentaItemViewModel> TiPuntoVentaItemViewModel()
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
    public class EstadoPuntoVenta
    {
        public string idEstado { get; set; }
        public string nombre { get; set; }
    }
}


