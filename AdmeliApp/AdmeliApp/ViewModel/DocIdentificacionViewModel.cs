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
    public class DocIdentificacionViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public DocIdentificacionItemViewModel CurrentDocIdentificacion { get; set; }

        private List<DocIdentificacion> DocIdentificacionList { get; set; }
        private ObservableCollection<DocIdentificacionItemViewModel> _DocIdentificacionItems;
        public ObservableCollection<DocIdentificacionItemViewModel> DocIdentificacionItems
        {
            get { return this._DocIdentificacionItems; }
            set { SetValue(ref this._DocIdentificacionItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public DocIdentificacionViewModel()
        {
            instance = this;
            this.CurrentDocIdentificacion = new DocIdentificacionItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.DocIdentificacionItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.DocIdentificacionItems = new ObservableCollection<DocIdentificacionItemViewModel>(
                    this.ToDocIdentificacionItemViewModel());
            }
            else
            {
                this.DocIdentificacionItems = new ObservableCollection<DocIdentificacionItemViewModel>(
                    this.ToDocIdentificacionItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentDocIdentificacion(new DocIdentificacionItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.DocIdentificacionPage.Navigation.PushAsync(new DocIdentificacionItemPage());
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/documentoidentificaciones/1/100
                RootObject<DocIdentificacion> rootData = await webService.GET<RootObject<DocIdentificacion>>("documentoidentificaciones", String.Format("{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.DocIdentificacionList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.DocIdentificacionItems = new ObservableCollection<DocIdentificacionItemViewModel>(
                    this.ToDocIdentificacionItemViewModel());
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

        internal void SetCurrentDocIdentificacion(DocIdentificacionItemViewModel docIdentificacionItemViewModel)
        {
            this.CurrentDocIdentificacion = docIdentificacionItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<DocIdentificacionItemViewModel> ToDocIdentificacionItemViewModel()
        {
            return DocIdentificacionList.Select(m => new DocIdentificacionItemViewModel
            {
                idDocumento = m.idDocumento,
                nombre = m.nombre,
                numeroDigitos = m.numeroDigitos,
                tipoDocumento = m.tipoDocumento,
                estado = m.estado,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static DocIdentificacionViewModel instance;

        public static DocIdentificacionViewModel GetInstance()
        {
            if (instance == null)
            {
                return new DocIdentificacionViewModel();
            }
            return instance;
        }
        #endregion
    }
}
