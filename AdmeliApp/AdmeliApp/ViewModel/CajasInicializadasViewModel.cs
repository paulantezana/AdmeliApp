using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ConfiguracionPages;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace AdmeliApp.ViewModel
{
    public class CajasInicializadasViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public CajasInicializadasItemViewModel CurrentCajasInicializadas { get; set; }

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

        private List<CajaSesion> CajasInicializadasList { get; set; }
        private ObservableCollection<CajasInicializadasItemViewModel> _CajasInicializadasItems;
        public ObservableCollection<CajasInicializadasItemViewModel> CajasInicializadasItems
        {
            get { return this._CajasInicializadasItems; }
            set { SetValue(ref this._CajasInicializadasItems, value); }
        }


        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public CajasInicializadasViewModel()
        {
            instance = this;
            this.CurrentCajasInicializadas = new CajasInicializadasItemViewModel();
            this.LoadRegisters();
            this.cargarSucursales();
        }

        #region =============================== COMMAND EXECUTE ===============================
        public override void ExecuteRefresh()
        {
            this.CajasInicializadasItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.CajasInicializadasItems = new ObservableCollection<CajasInicializadasItemViewModel>(
                    this.CajasInicializadasItemViewModel());
            }
            else
            {
                this.CajasInicializadasItems = new ObservableCollection<CajasInicializadasItemViewModel>(
                    this.CajasInicializadasItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentCajasInicializada(new CajasInicializadasItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.PuntoVentaPage.Navigation.PushAsync(new CajaInicializadaPage());
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

                // www.lineatienda.com/services.php/puntoventas/sucursal/0/estado/todos/1/100
                RootObject<CajaSesion> rootData = await webService.GET<RootObject<CajaSesion>>("cajasesionesinicializadas", String.Format("suc/{0}/{1}/{2}", idSucursal, paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.CajasInicializadasList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.CajasInicializadasItems = new ObservableCollection<CajasInicializadasItemViewModel>(
                    this.CajasInicializadasItemViewModel());
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

        internal void SetCurrentCajasInicializada(CajasInicializadasItemViewModel cajasInicializadasItemViewModel)
        {
            this.CurrentCajasInicializadas = cajasInicializadasItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<CajasInicializadasItemViewModel> CajasInicializadasItemViewModel()
        {
            return CajasInicializadasList.Select(c => new CajasInicializadasItemViewModel
            {
                nombres = c.nombres,
                apellidos = c.apellidos,
                idCajaSesion = c.idCajaSesion,
                fechaInicio = c.fechaInicio,
                fechaCierre = c.fechaCierre,
                estado = c.estado,
                totalIngreso = c.totalIngreso,
                totalEgreso = c.totalEgreso,
                nombre = c.nombre,

                BackgroundItem = (c.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (c.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static CajasInicializadasViewModel instance;

        public static CajasInicializadasViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CajasInicializadasViewModel();
            }
            return instance;
        }
        #endregion

    }
}
