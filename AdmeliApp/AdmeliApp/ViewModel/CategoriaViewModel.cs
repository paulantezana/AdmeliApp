using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class CategoriaViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        public ObservableCollection<CategoriaItemViewModel> CategoriaItems { get; set; }

        private ICommand refreshCategoriaCommand;
        public ICommand RefreshCategoriaCommand =>
            refreshCategoriaCommand ?? (refreshCategoriaCommand = new Command(() => ExecuteRefreshCategoriaAsync()));

        public CategoriaViewModel()
        {
            CategoriaItems = new ObservableCollection<CategoriaItemViewModel>();
            LoadCategoria();
        }

        private void ExecuteRefreshCategoriaAsync()
        {
            CategoriaItems.Clear();
            LoadCategoria();
        }

        private async void LoadCategoria()
        {
            try
            {
                IsRefreshing = true;
                // www.lineatienda.com/services.php/categoriastree
                RootObject<Categoria> rootData = await webService.GET<RootObject<Categoria>>("categoriastree");
                foreach (Categoria item in rootData.datos)
                {
                    CategoriaItems.Add(new CategoriaItemViewModel()
                    {
                        nombreCategoria = item.nombreCategoria,
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
