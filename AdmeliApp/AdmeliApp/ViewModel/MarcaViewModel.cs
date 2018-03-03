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
                IdMarca = m.IdMarca,
                NombreMarca = m.NombreMarca,
                SitioWeb = m.SitioWeb,
                Descripcion = m.Descripcion,
                Estado = m.Estado,
                CaptionImagen = m.CaptionImagen,
                UbicacionLogo = m.UbicacionLogo,
                TieneRegistros = m.TieneRegistros,

                BackgroundItem = (m.Estado == 0) ? (Color)App.Current.Resources["AlertLight"] : Color.Transparent,
                TextColorItem = (m.Estado == 0) ? (Color)App.Current.Resources["Alert"] : (Color)App.Current.Resources["GreyDark"],
            });
        }
    }
}