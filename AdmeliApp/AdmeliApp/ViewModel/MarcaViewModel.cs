using AdmeliApp.Helpers;
using AdmeliApp.ItemViewModel;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class MarcaViewModel : BaseViewModel
    {
        internal WebService webService = new WebService();

        private List<Marca> marcaList { get; set; }
        private ObservableCollection<MarcaItemViewModel> marcaItems;
        public ObservableCollection<MarcaItemViewModel> MarcaItems
        {
            get { return this.marcaItems; }
            set { SetValue(ref this.marcaItems, value); }
        }

        private string searchText;
        public string SearchText
        {
            get { return this.searchText; }
            set
            {
                SetValue(ref this.searchText, value);
                this.ExecuteSearch();
            }
        }

        #region ================= COMMANDS =================
        private ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(() => ExecuteRefresh()));

        private ICommand searchCommand;
        public ICommand SearchCommand =>
            searchCommand ?? (searchCommand = new Command(() => ExecuteSearch())); 
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
                        m => m.nombreMarca.ToLower().Contains(this.SearchText.ToLower())));
            }
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
                nombreMarca = m.nombreMarca,
            });
        }
    }
}