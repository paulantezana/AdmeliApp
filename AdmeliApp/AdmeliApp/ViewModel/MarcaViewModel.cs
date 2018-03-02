using AdmeliApp.Helpers;
using AdmeliApp.ItemViewModel;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.New;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class MarcaViewModel : BaseViewModel
    {
        internal WebService webService = new WebService();

        private List<Marca> marcaList { get; set; }
        private ObservableCollection<MarcaItemViewModel> _MarcaItems;
        public ObservableCollection<MarcaItemViewModel> MarcaItems
        {
            get { return this._MarcaItems; }
            set { SetValue(ref this._MarcaItems, value); }
        }

        private string _SearchText;
        public string SearchText
        {
            get { return this._SearchText; }
            set
            {
                SetValue(ref this._SearchText, value);
                this.ExecuteSearch();
            }
        }

        #region ================= COMMANDS =================
        private ICommand _RefreshCommand;
        public ICommand RefreshCommand =>
            _RefreshCommand ?? (_RefreshCommand = new Command(() => ExecuteRefresh()));

        private ICommand _SearchCommand;
        public ICommand SearchCommand =>
            _SearchCommand ?? (_SearchCommand = new Command(() => ExecuteSearch()));

        private ICommand _NuevoCommand;
        public ICommand NuevoCommand =>
            _NuevoCommand ?? (_NuevoCommand = new Command(() => ExecuteNuevo()));

        private ICommand _EditarCommand;
        public ICommand EditarCommand =>
            _EditarCommand ?? (_EditarCommand = new Command(() => ExecuteEditar()));

        private ICommand _AnularCommand;
        public ICommand AnularCommand =>
            _AnularCommand ?? (_AnularCommand = new Command(() => ExecuteAnular()));


        private ICommand _EliminarCommand;
        public ICommand EliminarCommand =>
            _EliminarCommand ?? (_EliminarCommand = new Command(() => ExecuteEliminar()));
        #endregion

        #region ================= CONSTRUCTOR =================
        public MarcaViewModel()
        {
            LoadMarca(1, 30);
        } 
        #endregion

        private void ExecuteRefresh()
        {
            MarcaItems.Clear();
            LoadMarca(1, 30);
        }

        private void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                this.MarcaItems = new ObservableCollection<MarcaItemViewModel>(
                    this.ToMarcaItemViewModel());
            }
            else
            {
                this.MarcaItems = new ObservableCollection<MarcaItemViewModel>(
                    this.ToMarcaItemViewModel().Where(
                        m => m.NombreMarca.ToLower().Contains(this.SearchText.ToLower())));
            }
        }

        private void ExecuteNuevo()
        {
            App.MarcaPage.Navigation.PushAsync(new NewMarcaPage());
        }

        private void ExecuteEditar()
        {
            throw new NotImplementedException();
        }

        private void ExecuteAnular()
        {
            throw new NotImplementedException();
        }

        private void ExecuteEliminar()
        {
            throw new NotImplementedException();
        }






        private async void LoadMarca(int page, int items)
        {
            try
            {
                IsRefreshing = true;
                // www.lineatienda.com/services.php/marcas/estado/1/100
                RootObject<Marca> rootData = await webService.GET<RootObject<Marca>>("marcas", String.Format("estado/{0}/{1}", page, items));
                marcaList = rootData.datos;

                this.MarcaItems = new ObservableCollection<MarcaItemViewModel>(
                    this.ToMarcaItemViewModel());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private IEnumerable<MarcaItemViewModel> ToMarcaItemViewModel()
        {
            return marcaList.Select(m => new MarcaItemViewModel
            {
                NombreMarca = m.NombreMarca,
            });
        }
    }
}