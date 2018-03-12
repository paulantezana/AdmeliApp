using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class CategoriaViewModel : BaseViewModelPagination
    {
        internal WebService webService = new WebService();

        public ObservableCollection<CategoriaItemViewModel> CategoriaItems { get; set; }

        private ICommand refreshCategoriaCommand;
        public ICommand RefreshCategoriaCommand =>
            refreshCategoriaCommand ?? (refreshCategoriaCommand = new Command(() => ExecuteRefreshCategoriaAsync()));

        public CategoriaViewModel()
        {
            CategoriaItems = new ObservableCollection<CategoriaItemViewModel>();
            this.LoadRegisters();
        }

        private void ExecuteRefreshCategoriaAsync()
        {
            CategoriaItems.Clear();
            this.LoadRegisters();
        }

        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/categoriastree
                RootObject<Categoria> rootData = await webService.GET<RootObject<Categoria>>("categoriastree");

                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

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
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                this.IsRefreshing = false;
                this.IsEnabled = true;
            }
        }

        #region =============================== SINGLETON ===============================
        private static CategoriaViewModel instance;

        public static CategoriaViewModel GetInstance()
        {
            if (CategoriaViewModel.instance == null)
            {
                return new CategoriaViewModel();
            }
            return CategoriaViewModel.instance;
        }
        #endregion

    }
}
