using AdmeliApp.Helpers;
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
    public class AlmacenViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public AlmacenItemViewModel CurrentAlmacen { get; set; }

        private List<Almacen> AlmacenList { get; set; }
        private ObservableCollection<AlmacenItemViewModel> _AlmacenItems;
        public ObservableCollection<AlmacenItemViewModel> AlmacenItems
        {
            get { return this._AlmacenItems; }
            set { SetValue(ref this._AlmacenItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public AlmacenViewModel()
        {
            instance = this;
            this.CurrentAlmacen = new AlmacenItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.AlmacenItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.AlmacenItems = new ObservableCollection<AlmacenItemViewModel>(
                    this.ToAlmacenItemViewModel());
            }
            else
            {
                this.AlmacenItems = new ObservableCollection<AlmacenItemViewModel>(
                    this.ToAlmacenItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentAlmacen(new AlmacenItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.AlmacenPage.Navigation.PushAsync(new AlmacenItemPage());
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;
                
                // www.lineatienda.com/services.php/almacenes/estado/1/100
                RootObject<Almacen> rootData = await webService.GET<RootObject<Almacen>>("almacenes", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.AlmacenList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.AlmacenItems = new ObservableCollection<AlmacenItemViewModel>(
                    this.ToAlmacenItemViewModel());
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

        internal void SetCurrentAlmacen(AlmacenItemViewModel almacenItemViewModel)
        {
            this.CurrentAlmacen = almacenItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<AlmacenItemViewModel> ToAlmacenItemViewModel()
        {
            return AlmacenList.Select(m => new AlmacenItemViewModel
            {
                idAlmacen = m.idAlmacen,
                nombre = m.nombre,
                direccion = m.direccion,
                principal = m.principal,
                estado = m.estado,
                idSucursal = m.idSucursal,
                idUbicacionGeografica = m.idUbicacionGeografica,
                nombreSucursal = m.nombreSucursal,
                tieneRegistros = m.tieneRegistros,
                idPersonalAlmacen = m.idPersonalAlmacen,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static AlmacenViewModel instance;

        public static AlmacenViewModel GetInstance()
        {
            if (instance == null)
            {
                return new AlmacenViewModel();
            }
            return instance;
        }
        #endregion
    }
}
