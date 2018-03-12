using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class CategoriaViewModel : BaseModel
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
                //this.LoadMarca();
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

        public ObservableCollection<CategoriaItemViewModel> CategoriaItems { get; set; }

        private ICommand refreshCategoriaCommand;
        public ICommand RefreshCategoriaCommand =>
            refreshCategoriaCommand ?? (refreshCategoriaCommand = new Command(() => ExecuteRefreshCategoriaAsync()));

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

        public CategoriaViewModel()
        {
            CategoriaItems = new ObservableCollection<CategoriaItemViewModel>();
            LoadCategoria();
        }

        private void ExecuteRefreshCategoriaAsync()
        {
            CategoriaItems.Clear();
            LoadCategoria();
        }

        private async void LoadCategoria()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/categoriastree
                RootObject<Categoria> rootData = await webService.GET<RootObject<Categoria>>("categoriastree");

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                foreach (Categoria item in rootData.datos)
                {
                    CategoriaItems.Add(new CategoriaItemViewModel()
                    {
                        nombreCategoria = item.nombreCategoria,
                    });
                }
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



    }
}
