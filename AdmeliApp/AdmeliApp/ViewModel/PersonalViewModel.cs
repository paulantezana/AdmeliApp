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
    public class PersonalViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public PersonalItemViewModel CurrentPersonal { get; set; }

        private List<Personal> PersonalList { get; set; }
        private ObservableCollection<PersonalItemViewModel> _PersonalItems;
        public ObservableCollection<PersonalItemViewModel> PersonalItems
        {
            get { return this._PersonalItems; }
            set { SetValue(ref this._PersonalItems, value); }
        }

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        public PersonalViewModel()
        {
            instance = this;
            this.CurrentPersonal = new PersonalItemViewModel();
            this.LoadRegisters();
        }

        public override void ExecuteRefresh()
        {
            this.PersonalItems.Clear();
            this.LoadRegisters();
        }

        public override void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.PersonalItems = new ObservableCollection<PersonalItemViewModel>(
                    this.ToMarcaItemViewModel());
            }
            else
            {
                this.PersonalItems = new ObservableCollection<PersonalItemViewModel>(
                    this.ToMarcaItemViewModel().Where(
                        m => m.nombres.ToLower().Contains(this.SearchText.ToLower())));
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

                RootObject<Personal> rootData = await webService.GET<RootObject<Personal>>("personales", String.Format("estado/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina));
                this.PersonalList = rootData.datos;

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

                // create observablecollection
                this.PersonalItems = new ObservableCollection<PersonalItemViewModel>(
                    this.ToMarcaItemViewModel());
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

        internal void SetCurrentPersonal(PersonalItemViewModel personalItemViewModel)
        {
            this.CurrentPersonal = personalItemViewModel;
        }
        #endregion

        #region ============================= CREATE OBJECT LIST =============================
        private IEnumerable<PersonalItemViewModel> ToMarcaItemViewModel()
        {
            return PersonalList.Select(m => new PersonalItemViewModel
            {
                idPersonal = m.idPersonal,
                nombres = m.nombres,
                apellidos = m.apellidos,
                fechaNacimiento = m.fechaNacimiento,
                tipoDocumento = m.tipoDocumento,
                numeroDocumento = m.numeroDocumento,
                sexo = m.sexo,
                email = m.email,
                telefono = m.telefono,
                celular = m.celular,
                usuario = m.usuario,
                password = m.password,
                direccion = m.direccion,

                estado = m.estado,
                idUbicacionGeografica = m.idUbicacionGeografica,
                idDocumento = m.idDocumento,

                BackgroundItem = (m.estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
        #endregion

        #region =============================== SINGLETON ===============================
        private static PersonalViewModel instance;

        public static PersonalViewModel GetInstance()
        {
            if (instance == null)
            {
                return new PersonalViewModel();
            }
            return instance;
        }
        #endregion
    }
}
