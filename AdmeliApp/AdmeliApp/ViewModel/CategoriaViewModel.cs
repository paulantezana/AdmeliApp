using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class CategoriaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public CategoriaItemViewModel CurrentCategoria { get; set; }

        private List<Categoria> categoriaList { get; set; }
        private ObservableCollection<CategoriaItemViewModel> _CategoriaItems;
        public ObservableCollection<CategoriaItemViewModel> CategoriaItems
        {
            get { return this._CategoriaItems; }
            set { SetValue(ref this._CategoriaItems, value); }
        }

        #region ============================= COMANDS =============================
        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));
        #endregion

        #region =========================== CONSTRUCTOR ===========================
        public CategoriaViewModel()
        {
            CategoriaViewModel.instance = this;
            this.CurrentCategoria = new CategoriaItemViewModel();
            this.LoadRegisters();
        }
        #endregion

        #region =========================== COMMAND EXECUTE ===========================
        public override void ExecuteRefresh()
        {
            this.CategoriaItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearchRealTime()
        {
            if (SearchText == string.Empty) this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.CategoriaItems = new ObservableCollection<CategoriaItemViewModel>(
                    this.ToCategoriaItemViewModel());
            }
            else
            {
                this.CategoriaItems = new ObservableCollection<CategoriaItemViewModel>(
                    this.ToCategoriaItemViewModel().Where(
                        m => m.nombreCategoria.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentCategoria(new CategoriaItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.CategoriaPage.Navigation.PushAsync(new CategoriaItemPage()); // Navegacion
        }
        #endregion

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/categoriastree
                RootObject<Categoria> rootData = await webService.GET<RootObject<Categoria>>("categoriastree");
                this.categoriaList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.CategoriaItems = new ObservableCollection<CategoriaItemViewModel>(
                    this.ToCategoriaItemViewModel());
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

        internal void SetCurrentCategoria(CategoriaItemViewModel categoriaItemViewModel)
        {
            this.CurrentCategoria = categoriaItemViewModel;
        } 
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<CategoriaItemViewModel> ToCategoriaItemViewModel()
        {
            return categoriaList.Select(c => new CategoriaItemViewModel
            {
                idCategoria = c.idCategoria,
                nombreCategoria = c.nombreCategoria,
                idPadreCategoria = c.idPadreCategoria,
                padre = c.padre,
                ordenVisualizacionProductos = c.ordenVisualizacionProductos,
                mostrarProductosEn = c.mostrarProductosEn,
                numeroColumnas = c.numeroColumnas,
                tituloCategoriaSeo = c.tituloCategoriaSeo,
                urlCategoriaSeo = c.urlCategoriaSeo,
                metaTagsSeo = c.metaTagsSeo,
                cabeceraPagina = c.cabeceraPagina,
                piePagina = c.piePagina,
                orden = c.orden,
                estado = c.estado,
                mostrarWeb = c.mostrarWeb,
                tieneRegistros = c.tieneRegistros,
                relacionPrincipal = c.relacionPrincipal,
                afecta = c.afecta,

                BackgroundItem = (c.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (c.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static CategoriaViewModel instance;

        public static CategoriaViewModel GetInstance()
        {
            if (CategoriaViewModel.instance == null)
            {
                return new CategoriaViewModel();
            }
            return CategoriaViewModel.instance;
        }
        #endregion

    }
}
