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
    public class DenominacionViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public DenominacionItemViewModel CurrentDenominacion { get; set; }

        private List<Denominacion> DenominacionList { get; set; }
        private ObservableCollection<DenominacionItemViewModel> _DenominacionItems;
        public ObservableCollection<DenominacionItemViewModel> DenominacionItems
        {
            get { return this._DenominacionItems; }
            set { SetValue(ref this._DenominacionItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public DenominacionViewModel()
        {
            instance = this;
            this.CurrentDenominacion = new DenominacionItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.DenominacionItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.DenominacionItems = new ObservableCollection<DenominacionItemViewModel>(
                    this.ToDenominacionItemViewModel());
            }
            else
            {
                this.DenominacionItems = new ObservableCollection<DenominacionItemViewModel>(
                    this.ToDenominacionItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentDenominacion(new DenominacionItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.DenominacionPage.Navigation.PushAsync(new DenominacionItemPage());
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/marcas/estado/1/100
                RootObject<Denominacion> rootData = await webService.GET<RootObject<Denominacion>>("denominaciones", String.Format("paginacion/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.DenominacionList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.DenominacionItems = new ObservableCollection<DenominacionItemViewModel>(
                    this.ToDenominacionItemViewModel());
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

        internal void SetCurrentDenominacion(DenominacionItemViewModel denominacionItemViewModel)
        {
            this.CurrentDenominacion = denominacionItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<DenominacionItemViewModel> ToDenominacionItemViewModel()
        {
            return DenominacionList.Select(m => new DenominacionItemViewModel
            {
                idDenominacion = m.idDenominacion,
                tipoMoneda = m.tipoMoneda,
                nombre = m.nombre,
                valor = m.valor,
                imagen = m.imagen,
                estado = m.estado,
                idMoneda = m.idMoneda,
                moneda = m.moneda,
                anular = m.anular,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static DenominacionViewModel instance;

        public static DenominacionViewModel GetInstance()
        {
            if (instance == null)
            {
                return new DenominacionViewModel();
            }
            return instance;
        }
        #endregion
    }
}
