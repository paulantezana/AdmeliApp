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
    public class MarcaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        #region =================================== COLLECTIONS ===================================
        private List<Marca> marcaList { get; set; }
        private ObservableCollection<MarcaItemViewModel> _MarcaItems;
        public ObservableCollection<MarcaItemViewModel> MarcaItems
        {
            get { return this._MarcaItems; }
            set { SetValue(ref this._MarcaItems, value); }
        }
        #endregion

        #region =============================== PROPERTIES ===============================
        public MarcaItemViewModel CurrentMarca { get; set; }
        private string _SearchText;
        public string SearchText
        {
            get { return this._SearchText; }
            set
            {
                SetValue(ref this._SearchText, value);
                this.ExecuteSearch();
            }
        } 
        #endregion

        #region ================================ COMMANDS ================================
        private ICommand _RefreshCommand;
        public ICommand RefreshCommand =>
            _RefreshCommand ?? (_RefreshCommand = new Command(() => ExecuteRefresh()));

        private ICommand _SearchCommand;
        public ICommand SearchCommand =>
            _SearchCommand ?? (_SearchCommand = new Command(() => ExecuteSearch()));

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));
        #endregion

        #region =========================== CONSTRUCTOR ===========================
        public MarcaViewModel()
        {
            MarcaViewModel.instance = this;
            this.CurrentMarca = new MarcaItemViewModel();
            this.LoadRegisters();
        }
        #endregion

        #region =========================== COMMAND EXECUTE ===========================
        internal void ExecuteRefresh()
        {
            this.MarcaItems.Clear();
            this.LoadRegisters();
        }

        private void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.MarcaItems = new ObservableCollection<MarcaItemViewModel>(
                    this.ToMarcaItemViewModel());
            }
            else
            {
                this.MarcaItems = new ObservableCollection<MarcaItemViewModel>(
                    this.ToMarcaItemViewModel().Where(
                        m => m.NombreMarca.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentMarca(new MarcaItemViewModel() { Nuevo = true, DeleteIsEnabled =  false });
            App.MarcaPage.Navigation.PushAsync(new MarcaItemPage()); // Navegacion
        }
        #endregion

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/marcas/estado/1/100
                RootObject<Marca> rootData = await webService.GET<RootObject<Marca>>("marcas", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.marcaList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.MarcaItems = new ObservableCollection<MarcaItemViewModel>(
                    this.ToMarcaItemViewModel());
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

        internal void SetCurrentMarca(MarcaItemViewModel marcaItemViewModel)
        {
            this.CurrentMarca = marcaItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<MarcaItemViewModel> ToMarcaItemViewModel()
        {
            return marcaList.Select(m => new MarcaItemViewModel
            {
                IdMarca = m.IdMarca,
                NombreMarca = m.NombreMarca,
                SitioWeb = m.SitioWeb,
                Descripcion = m.Descripcion,
                Estado = m.Estado,
                CaptionImagen = m.CaptionImagen,
                UbicacionLogo = m.UbicacionLogo,
                TieneRegistros = m.TieneRegistros,

                BackgroundItem = (m.Estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.Estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        } 
        #endregion

        #region =============================== SINGLETON ===============================
        private static MarcaViewModel instance;

        public static MarcaViewModel GetInstance()
        {
            if (MarcaViewModel.instance == null)
            {
                return new MarcaViewModel();
            }
            return MarcaViewModel.instance;
        }
        #endregion
    }
}