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
    public class ImpuestoViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public ImpuestoItemViewModel CurrentImpuesto { get; set; }

        private List<Impuesto> ImpuestoList { get; set; }
        private ObservableCollection<ImpuestoItemViewModel> _ImpuestoItems;
        public ObservableCollection<ImpuestoItemViewModel> ImpuestoItems
        {
            get { return this._ImpuestoItems; }
            set { SetValue(ref this._ImpuestoItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public ImpuestoViewModel()
        {
            instance = this;
            this.CurrentImpuesto = new ImpuestoItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.ImpuestoItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.ImpuestoItems = new ObservableCollection<ImpuestoItemViewModel>(
                    this.ToImpuestoItemViewModel());
            }
            else
            {
                this.ImpuestoItems = new ObservableCollection<ImpuestoItemViewModel>(
                    this.ToImpuestoItemViewModel().Where(
                        m => m.nombreImpuesto.ToLower().Contains(this.SearchText.ToLower())));
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
                
                // www.lineatienda.com/services.php/impuestos/estado/1/100
                RootObject<Impuesto> rootData = await webService.GET<RootObject<Impuesto>>("impuestos", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.ImpuestoList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.ImpuestoItems = new ObservableCollection<ImpuestoItemViewModel>(
                    this.ToImpuestoItemViewModel());
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

        internal void SetCurrentMarca(ImpuestoItemViewModel impuestoItemViewModel)
        {
            this.CurrentImpuesto = impuestoItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<ImpuestoItemViewModel> ToImpuestoItemViewModel()
        {
            return ImpuestoList.Select(m => new ImpuestoItemViewModel
            {
                idImpuesto = m.idImpuesto,
                nombreImpuesto = m.nombreImpuesto,
                siglasImpuesto = m.siglasImpuesto,
                valorImpuesto = m.valorImpuesto,
                porcentual = m.porcentual,
                porDefecto = m.porDefecto,
                estado = m.estado,
                enUso = m.enUso,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static ImpuestoViewModel instance;

        public static ImpuestoViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ImpuestoViewModel();
            }
            return instance;
        }
        #endregion
    }
}
