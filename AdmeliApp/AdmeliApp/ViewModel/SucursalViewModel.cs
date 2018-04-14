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
    public class SucursalViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public SucursalItemViewModel CurrentSucursal { get; set; }

        private List<Sucursal> SucursalList { get; set; }
        private ObservableCollection<SucursalItemViewModel> _SucursalItems;
        public ObservableCollection<SucursalItemViewModel> SucursalItems
        {
            get { return this._SucursalItems; }
            set { SetValue(ref this._SucursalItems, value); }
        }

        #region ============================ COMMANDS ============================
        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo())); 
        #endregion

        #region ========================== CONSTRUCTOR ==========================
        public SucursalViewModel()
        {
            instance = this;
            this.CurrentSucursal = new SucursalItemViewModel();
            this.LoadRegisters();
        }
        #endregion

        #region =========================== COMMAND EXECUTE ===========================
        public override void ExecuteRefresh()
        {
            this.SucursalItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.SucursalItems = new ObservableCollection<SucursalItemViewModel>(
                    this.ToSucursalItemViewModel());
            }
            else
            {
                this.SucursalItems = new ObservableCollection<SucursalItemViewModel>(
                    this.ToSucursalItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentSucursal(new SucursalItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.SucursalPage.Navigation.PushAsync(new SucursalItemPage());
        } 
        #endregion

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/sucursales/estado/1/100
                RootObject<Sucursal> rootData = await webService.GET<RootObject<Sucursal>>("sucursales", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.SucursalList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.SucursalItems = new ObservableCollection<SucursalItemViewModel>(
                    this.ToSucursalItemViewModel());
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

        internal void SetCurrentSucursal(SucursalItemViewModel sucursalItemViewModel)
        {
            this.CurrentSucursal = sucursalItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<SucursalItemViewModel> ToSucursalItemViewModel()
        {
            return SucursalList.Select(m => new SucursalItemViewModel
            {
                idSucursal = m.idSucursal,
                nombre = m.nombre,
                principal = m.principal,
                estado = m.estado,
                estados = m.estados,
                direccion = m.direccion,
                idUbicacionGeografica = m.idUbicacionGeografica,
                tieneRegistros = m.tieneRegistros,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static SucursalViewModel instance;

        public static SucursalViewModel GetInstance()
        {
            if (instance == null)
            {
                return new SucursalViewModel();
            }
            return instance;
        }
        #endregion
    }
}
