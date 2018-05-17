using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.CompraPages.CompraItemPages;
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
    public class ProveedorViewModel : BaseViewModelPagination
    {

        internal WebService webService = new WebService();

        public ProveedorItemViewModel CurrentProveedor { get; set; }

        // Lista principal
        private List<Proveedor> ProveedorList { get; set; }
        private ObservableCollection<ProveedorItemViewModel> _ProveedorItems;
        public ObservableCollection<ProveedorItemViewModel> ProveedorItems
        {
            get { return this._ProveedorItems; }
            set { SetValue(ref this._ProveedorItems, value); }
        }

        // Commands
        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public override void ExecuteRefresh()
        {
            this.ProveedorItems.Clear();
            this.LoadRegisters();
        }

        public ProveedorViewModel()
        {
            instance = this;
            this.CurrentProveedor = new ProveedorItemViewModel();
            this.LoadRegisters();
        }


        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.ProveedorItems = new ObservableCollection<ProveedorItemViewModel>(
                    this.ToProveedorItemViewModel());
            }
            else
            {
                this.ProveedorItems = new ObservableCollection<ProveedorItemViewModel>(
                    this.ToProveedorItemViewModel().Where(
                        m => m.razonSocial.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentProveedor(new ProveedorItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.ProveedorPage.Navigation.PushAsync(new ProveedorItemPage());
        }

        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/ventas/sucursal/0/puntoventa/0/per/0/estado/todos/1/100
                RootObject<Proveedor> rootData = await webService.GET<RootObject<Proveedor>>("proveedores", String.Format("{0}/{1}/{2}", "estado", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.ProveedorList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.ProveedorItems = new ObservableCollection<ProveedorItemViewModel>(
                    this.ToProveedorItemViewModel());
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

        internal void SetCurrentProveedor(ProveedorItemViewModel proveedorItemViewModel)
        {
            this.CurrentProveedor = proveedorItemViewModel;
        }

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<ProveedorItemViewModel> ToProveedorItemViewModel()
        {
            return ProveedorList.Select(p => new ProveedorItemViewModel
            {
                idProveedor = p.idProveedor,
                ruc = p.ruc,
                razonSocial = p.razonSocial,
                telefono = p.telefono,
                email = p.email,
                actividadPrincipal = p.actividadPrincipal,
                tipoProveedor = p.tipoProveedor,
                direccion = p.direccion,
                estado = p.estado,
                idUbicacionGeografica = p.idUbicacionGeografica,
                NroCompras = p.NroCompras,

                BackgroundItem = (p.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (p.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static ProveedorViewModel instance;

        public static ProveedorViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProveedorViewModel();
            }
            return instance;
        }
        #endregion
    }
}
