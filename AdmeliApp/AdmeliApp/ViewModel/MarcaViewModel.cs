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
    public class MarcaViewModel : BaseModel
    {
        internal WebService webService = new WebService();
        internal Paginacion paginacion = new Paginacion(1, App.configuracionGeneral.itemPorPagina);

        private int _CurrentPage;
        public int CurrentPage
        {
            get { return this._CurrentPage; }
            set
            {
                SetValue(ref this._CurrentPage, value);
                this.paginacion.reloadPage((value == 0) ? 1 : value);
                this.LoadMarca();
            }
        }

        private string _NRegistros;
        public string NRegistros
        {
            get { return this._NRegistros; }
            set { SetValue(ref this._NRegistros, value); }
        }

        private bool _FirstIsVisible;
        public bool FirstIsVisible
        {
            get { return this._FirstIsVisible; }
            set { SetValue(ref this._FirstIsVisible, value); }
        }

        private bool _PreviousIsVisible;
        public bool PreviousIsVisible
        {
            get { return this._PreviousIsVisible; }
            set { SetValue(ref this._PreviousIsVisible, value); }
        }

        private bool _NextIsVisible;
        public bool NextIsVisible
        {
            get { return this._NextIsVisible; }
            set { SetValue(ref this._NextIsVisible, value); }
        }

        private bool _LastIsVisible;
        public bool LastIsVisible
        {
            get { return this._LastIsVisible; }
            set { SetValue(ref this._LastIsVisible, value); }
        }

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

        private ICommand _FirstCommand;
        public ICommand FirstCommand =>
            _FirstCommand ?? (_FirstCommand = new Command(() => ExecuteFirstPage()));

        private ICommand _PreviousCommand;
        public ICommand PreviousCommand =>
            _PreviousCommand ?? (_PreviousCommand = new Command(() => ExecutePreviousPage()));

        private ICommand _NextCommand;
        public ICommand NextCommand =>
            _NextCommand ?? (_NextCommand = new Command(() => ExecuteNextPage()));

        private ICommand _LastCommand;
        public ICommand LastCommand =>
            _LastCommand ?? (_LastCommand = new Command(() => ExecutelastPage()));
        #endregion

        #region =========================== CONSTRUCTOR ===========================
        public MarcaViewModel()
        {
            MarcaViewModel.instance = this;
            this.CurrentMarca = new MarcaItemViewModel();
            this.LoadMarca();
        }
        #endregion

        #region =========================== COMMAND EXECUTE ===========================
        internal void ExecuteRefresh()
        {
            this.MarcaItems.Clear();
            this.LoadMarca();
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

        private void ExecuteFirstPage()
        {
            if (this.CurrentPage != 1)
            {
                this.paginacion.firstPage();
                this.LoadMarca();
            }
        }

        private void ExecutePreviousPage()
        {
            if (this.CurrentPage != 1)
            {
                this.paginacion.previousPage();
                this.LoadMarca();
            }
        }

        private void ExecuteNextPage()
        {
            if (this.paginacion.pageCount != this.CurrentPage)
            {
                this.paginacion.nextPage();
                this.LoadMarca();
            }
        }

        private void ExecutelastPage()
        {
            if (this.paginacion.pageCount != this.CurrentPage)
            {
                this.paginacion.lastPage();
                this.LoadMarca();
            }
        }
        #endregion

        #region ===================================== LOADS =====================================
        private async void LoadMarca()
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

        private void reloadPagination()
        {
            // Print data pagination
            this.CurrentPage = paginacion.currentPage;
            this.NRegistros = String.Format("{0} Registros de {1} ", App.configuracionGeneral.itemPorPagina, this.paginacion.itemsCount);

            // Pagination button navigation is visible false or true
            this.NextIsVisible = ((this.paginacion.pageCount - this.paginacion.currentPage) >= 1);
            this.LastIsVisible = ((this.paginacion.pageCount - this.paginacion.currentPage) >= 2);
            this.PreviousIsVisible = (this.paginacion.currentPage >= 2);
            this.FirstIsVisible = (this.paginacion.currentPage >= 3);
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