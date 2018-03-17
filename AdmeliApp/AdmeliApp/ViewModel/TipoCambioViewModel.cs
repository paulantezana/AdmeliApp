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
    public class TipoCambioViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public TipoCambioItemViewModel CurrentTipoCambio { get; set; }

        private List<TipoCambioMoneda> TipoCambioList { get; set; }
        private ObservableCollection<TipoCambioItemViewModel> _TipoCambioItems;
        public ObservableCollection<TipoCambioItemViewModel> TipoCambioItems
        {
            get { return this._TipoCambioItems; }
            set { SetValue(ref this._TipoCambioItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public TipoCambioViewModel()
        {
            instance = this;
            this.CurrentTipoCambio = new TipoCambioItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.TipoCambioItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.TipoCambioItems = new ObservableCollection<TipoCambioItemViewModel>(
                    this.ToTipoCambioItemViewModel());
            }
            else
            {
                this.TipoCambioItems = new ObservableCollection<TipoCambioItemViewModel>(
                    this.ToTipoCambioItemViewModel().Where(
                        m => m.moneda.ToLower().Contains(this.SearchText.ToLower())));
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

                // www.lineatienda.com/services.php/marcas/estado/1/100
                RootObject<TipoCambioMoneda> rootData = await webService.GET<RootObject<TipoCambioMoneda>>("marcas", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.TipoCambioList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.TipoCambioItems = new ObservableCollection<TipoCambioItemViewModel>(
                    this.ToTipoCambioItemViewModel());
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

        internal void SetCurrentMarca(TipoCambioItemViewModel tipoCambioItemViewModel)
        {
            this.CurrentTipoCambio = tipoCambioItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<TipoCambioItemViewModel> ToTipoCambioItemViewModel()
        {
            return TipoCambioList.Select(m => new TipoCambioItemViewModel
            {
                moneda = m.moneda,
                cambio = m.cambio,
                
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static TipoCambioViewModel instance;

        public static TipoCambioViewModel GetInstance()
        {
            if (instance == null)
            {
                return new TipoCambioViewModel();
            }
            return instance;
        }
        #endregion
    }
}
