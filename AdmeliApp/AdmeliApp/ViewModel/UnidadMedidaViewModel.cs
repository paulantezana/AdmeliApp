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
    public class UnidadMedidaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        #region =================================== COLLECTIONS ===================================
        private List<UnidadMedida> unidadMedidaList { get; set; }
        private ObservableCollection<UnidadMedidaItemViewModel> _UnidadMedidaItems;
        public ObservableCollection<UnidadMedidaItemViewModel> UnidadMedidaItems
        {
            get { return this._UnidadMedidaItems; }
            set { SetValue(ref this._UnidadMedidaItems, value); }
        } 
        #endregion

        #region ===================================== COMANDS =====================================
        private ICommand refreshUnidadMedidaCommand;
        public ICommand RefreshUnidadMedidaCommand =>
            refreshUnidadMedidaCommand ?? (refreshUnidadMedidaCommand = new Command(() => ExecuteRefreshUnidadMedidaAsync()));
        #endregion

        private void ExecuteRefreshUnidadMedidaAsync()
        {
            UnidadMedidaItems.Clear();
            this.LoadRegisters();
        }

        public UnidadMedidaViewModel()
        {
            UnidadMedidaItems = new ObservableCollection<UnidadMedidaItemViewModel>();
            this.LoadRegisters();
        }

        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/unimedidas/estado/1/100
                RootObject<UnidadMedida> rootData = await webService.GET<RootObject<UnidadMedida>>("unimedidas", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));

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

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<UnidadMedidaItemViewModel> ToMarcaItemViewModel()
        {
            return unidadMedidaList.Select(m => new UnidadMedidaItemViewModel
            {
                idUnidadMedida = m.idUnidadMedida,
                nombreUnidad = m.nombreUnidad,
                simbolo = m.simbolo,
                estado = m.estado,
                tieneRegistros = m.tieneRegistros,
                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static UnidadMedidaViewModel instance;

        public static UnidadMedidaViewModel GetInstance()
        {
            if (UnidadMedidaViewModel.instance == null)
            {
                return new UnidadMedidaViewModel();
            }
            return UnidadMedidaViewModel.instance;
        }
        #endregion

    }
}
