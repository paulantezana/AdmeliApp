using AdmeliApp.Helpers;
using AdmeliApp.Model;
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
    public class TipoDocumentoViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public TipoDocumentoItemViewModel CurrentTipoDocumento { get; set; }

        private List<TipoDocumento> TipoDocumentoList { get; set; }
        private ObservableCollection<TipoDocumentoItemViewModel> _TipoDocumentoItems;
        public ObservableCollection<TipoDocumentoItemViewModel> TipoDocumentoItems
        {
            get { return this._TipoDocumentoItems; }
            set { SetValue(ref this._TipoDocumentoItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public TipoDocumentoViewModel()
        {
            instance = this;
            this.CurrentTipoDocumento = new TipoDocumentoItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.TipoDocumentoItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.TipoDocumentoItems = new ObservableCollection<TipoDocumentoItemViewModel>(
                    this.ToTipoDocumentoItemViewModel());
            }
            else
            {
                this.TipoDocumentoItems = new ObservableCollection<TipoDocumentoItemViewModel>(
                    this.ToTipoDocumentoItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            throw new NotImplementedException();
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/tipodocumentos/1/100
                RootObject<TipoDocumento> rootData = await webService.GET<RootObject<TipoDocumento>>("tipodocumentos", String.Format("{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.TipoDocumentoList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.TipoDocumentoItems = new ObservableCollection<TipoDocumentoItemViewModel>(
                    this.ToTipoDocumentoItemViewModel());
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

        internal void SetCurrentMarca(TipoDocumentoItemViewModel tipoDocumentoItemViewModel)
        {
            this.CurrentTipoDocumento = tipoDocumentoItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<TipoDocumentoItemViewModel> ToTipoDocumentoItemViewModel()
        {
            return TipoDocumentoList.Select(m => new TipoDocumentoItemViewModel
            {
                idTipoDocumento = m.idTipoDocumento,
                nombre = m.nombre,
                nombreLabel = m.nombreLabel,
                descripcion = m.descripcion,
                comprobante = m.comprobante,
                area = m.area,
                tipoCliente = m.tipoCliente,
                estado = m.estado,
                formatoDocumento = m.formatoDocumento,
                redimensionarModelo = m.redimensionarModelo,
                bordeDetalle = m.bordeDetalle,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static TipoDocumentoViewModel instance;

        public static TipoDocumentoViewModel GetInstance()
        {
            if (instance == null)
            {
                return new TipoDocumentoViewModel();
            }
            return instance;
        }
        #endregion
    }
}
