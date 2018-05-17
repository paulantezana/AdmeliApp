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
    public class MonedaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public MonedaItemViewModel CurrentMoneda { get; set; }

        private List<Moneda> MonedaList { get; set; }
        private ObservableCollection<MonedaItemViewModel> _MonedaItems;
        public ObservableCollection<MonedaItemViewModel> MonedaItems
        {
            get { return this._MonedaItems; }
            set { SetValue(ref this._MonedaItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public MonedaViewModel()
        {
            instance = this;
            this.CurrentMoneda = new MonedaItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.MonedaItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.MonedaItems = new ObservableCollection<MonedaItemViewModel>(
                    this.ToMonedaItemViewModel());
            }
            else
            {
                this.MonedaItems = new ObservableCollection<MonedaItemViewModel>(
                    this.ToMonedaItemViewModel().Where(
                        m => m.moneda.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentMoneda(new MonedaItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.MonedaPage.Navigation.PushAsync(new MonedaItemPage());
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/monedas/estado/1/100
                RootObject<Moneda> rootData = await webService.GET<RootObject<Moneda>>("monedas", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.MonedaList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.MonedaItems = new ObservableCollection<MonedaItemViewModel>(
                    this.ToMonedaItemViewModel());
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

        internal void SetCurrentMoneda(MonedaItemViewModel marcaItemViewModel)
        {
            this.CurrentMoneda = marcaItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<MonedaItemViewModel> ToMonedaItemViewModel()
        {
            return MonedaList.Select(m => new MonedaItemViewModel
            {
                idMoneda = m.idMoneda,
                idMonedaPorDefecto = m.idMonedaPorDefecto,
                moneda = m.moneda,
                simbolo = m.simbolo,
                porDefecto = m.porDefecto,
                estado = m.estado,
                tipoCambio = m.tipoCambio,
                fechaCreacion = m.fechaCreacion,
                idPersonal = m.idPersonal,
                nombres = m.nombres,
                apellidos = m.apellidos,
                tieneRegistros = m.tieneRegistros,
                fechaPago = m.fechaPago,
                idCaja = m.idCaja,
                idCajaSesion = m.idCajaSesion,
                idMedioPago = m.idMedioPago,
                medioPago = m.medioPago,
                monto = m.monto,
                motivo = m.motivo,
                numeroOperacion = m.numeroOperacion,
                observacion = m.observacion,
                total = m.total,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static MonedaViewModel instance;

        public static MonedaViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MonedaViewModel();
            }
            return instance;
        }
        #endregion
    }
}
