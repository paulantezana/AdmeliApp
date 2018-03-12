using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class UnidadMedidaViewModel : BaseModel
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
                this.LoadUnidadMedida(1,2);
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

        public ObservableCollection<UnidadMedidaItemViewModel> UnidadMedidaItems { get; set; }

        #region ===================================== COMANDS =====================================
        private ICommand refreshUnidadMedidaCommand;
        public ICommand RefreshUnidadMedidaCommand =>
            refreshUnidadMedidaCommand ?? (refreshUnidadMedidaCommand = new Command(() => ExecuteRefreshUnidadMedidaAsync()));

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

        private void ExecuteRefreshUnidadMedidaAsync()
        {
            UnidadMedidaItems.Clear();
            LoadUnidadMedida(1, 30);
        }

        public UnidadMedidaViewModel()
        {
            UnidadMedidaItems = new ObservableCollection<UnidadMedidaItemViewModel>();
            LoadUnidadMedida(1, 30);
        }

        private async void LoadUnidadMedida(int page, int items)
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/unimedidas/estado/1/100
                RootObject<UnidadMedida> rootData = await webService.GET<RootObject<UnidadMedida>>("unimedidas", String.Format("estado/{0}/{1}", page, items));

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                foreach (UnidadMedida item in rootData.datos)
                {
                    UnidadMedidaItems.Add(new UnidadMedidaItemViewModel()
                    {
                        nombreUnidad = item.nombreUnidad,
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


        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<UnidadMedidaItemViewModel> ToMarcaItemViewModel()
        {
            return marcaList.Select(m => new UnidadMedidaItemViewModel
            {
               /* IdMarca = m.IdMarca,
                NombreMarca = m.NombreMarca,
                SitioWeb = m.SitioWeb,
                Descripcion = m.Descripcion,
                Estado = m.Estado,
                CaptionImagen = m.CaptionImagen,
                UbicacionLogo = m.UbicacionLogo,
                TieneRegistros = m.TieneRegistros,

                BackgroundItem = (m.Estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.Estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],*/
            });
        }
        #endregion

    }
}
