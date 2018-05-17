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
    public class GrupoClienteViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public GrupoClienteItemViewModel CurrentGrupoCliente { get; set; }

        private List<GrupoCliente> GrupoClienteList { get; set; }
        private ObservableCollection<GrupoClienteItemViewModel> _GrupoClienteItems;
        public ObservableCollection<GrupoClienteItemViewModel> GrupoClienteItems
        {
            get { return this._GrupoClienteItems; }
            set { SetValue(ref this._GrupoClienteItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public GrupoClienteViewModel()
        {
            instance = this;
            this.CurrentGrupoCliente = new GrupoClienteItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.GrupoClienteItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.GrupoClienteItems = new ObservableCollection<GrupoClienteItemViewModel>(
                    this.ToGrupoClienteItemViewModel());
            }
            else
            {
                this.GrupoClienteItems = new ObservableCollection<GrupoClienteItemViewModel>(
                    this.ToGrupoClienteItemViewModel().Where(
                        m => m.nombreGrupo.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentGrupoCliente(new GrupoClienteItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.GrupoClientePage.Navigation.PushAsync(new GrupoClienteItemPage());
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;
                
                // www.lineatienda.com/services.php/gclientes/estado/1/100
                RootObject<GrupoCliente> rootData = await webService.GET<RootObject<GrupoCliente>>("gclientes", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.GrupoClienteList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.GrupoClienteItems = new ObservableCollection<GrupoClienteItemViewModel>(
                    this.ToGrupoClienteItemViewModel());
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

        internal void SetCurrentGrupoCliente(GrupoClienteItemViewModel grupoClienteItemViewModel)
        {
            this.CurrentGrupoCliente = grupoClienteItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<GrupoClienteItemViewModel> ToGrupoClienteItemViewModel()
        {
            return GrupoClienteList.Select(m => new GrupoClienteItemViewModel
            {
                idGrupoCliente = m.idGrupoCliente,
                nombreGrupo = m.nombreGrupo,
                descripcion = m.descripcion,
                minimoOrden = m.minimoOrden,
                estado = m.estado,
                enUso = m.enUso,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static GrupoClienteViewModel instance;

        public static GrupoClienteViewModel GetInstance()
        {
            if (instance == null)
            {
                return new GrupoClienteViewModel();
            }
            return instance;
        }
        #endregion
    }
}
