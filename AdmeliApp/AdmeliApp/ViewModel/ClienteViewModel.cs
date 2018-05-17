using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.VentaPages.VentaItemPages;
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
    public class ClienteViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public ClienteItemViewModel CurrentCliente { get; set; }

        private List<Cliente> ClienteList { get; set; }
        private ObservableCollection<ClienteItemViewModel> _ClienteItems;
        public ObservableCollection<ClienteItemViewModel> ClienteItems
        {
            get { return this._ClienteItems; }
            set { SetValue(ref this._ClienteItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public ClienteViewModel()
        {
            instance = this;
            this.CurrentCliente = new ClienteItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.ClienteItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.ClienteItems = new ObservableCollection<ClienteItemViewModel>(
                    this.ToClienteItemViewModel());
            }
            else
            {
                this.ClienteItems = new ObservableCollection<ClienteItemViewModel>(
                    this.ToClienteItemViewModel().Where(
                        m => m.nombre.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            this.SetCurrentCliente(new ClienteItemViewModel() { Nuevo = true, DeleteIsEnabled = false });
            App.ClientePage.Navigation.PushAsync(new ClienteItemPage());
        }

        #region ===================================== LOADS =====================================
        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/clientes/estado/1/100
                RootObject<Cliente> rootData = await webService.GET<RootObject<Cliente>>("clientes", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.ClienteList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.ClienteItems = new ObservableCollection<ClienteItemViewModel>(
                    this.ToClienteItemViewModel());
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

        internal void SetCurrentCliente(ClienteItemViewModel clienteItemViewModel)
        {
            this.CurrentCliente = clienteItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<ClienteItemViewModel> ToClienteItemViewModel()
        {
            return ClienteList.Select(m => new ClienteItemViewModel
            {
                idCliente = m.idCliente,
                nombreCliente = m.nombreCliente,
                numeroDocumento = m.numeroDocumento,
                email = m.email,
                telefono = m.telefono,
                celular = m.celular,
                sexo = m.sexo,
                direccion = m.direccion,
                zipCode = m.zipCode,
                esEventual = m.esEventual,
                observacion = m.observacion,
                estado = m.estado,
                idUbicacionGeografica = m.idUbicacionGeografica,
                idGrupoCliente = m.idGrupoCliente,
                nombreGrupo = m.nombreGrupo,
                nroVentasCotizaciones = m.nroVentasCotizaciones,
                idDocumento = m.idDocumento,
                nombre = m.nombre,
                tipoDocumento = m.tipoDocumento,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static ClienteViewModel instance;

        public static ClienteViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ClienteViewModel();
            }
            return instance;
        }
        #endregion
    }
}
