using AdmeliApp.Helpers;
using AdmeliApp.ItemViewModel;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class ProductoViewModel : BaseViewModel
    {
        internal WebService webService = new WebService();

        public ObservableCollection<ProductoItemViewModel> ProductoItems { get; set; }

        public string SearchText { get; set; }

        private ICommand refreshProductoCommand;
        public ICommand RefreshProductoCommand =>
            refreshProductoCommand ?? (refreshProductoCommand = new Command(() => ExecuteRefreshProductoAsync()));

        private void ExecuteRefreshProductoAsync()
        {
            ProductoItems.Clear();
            Dictionary<string, int> listReSend = new Dictionary<string, int>();
            listReSend.Add("id0", 0);
            //Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;
            LoadProducto(listReSend, 1, 30);
        }

        public ICommand SearchCommand { get; set; }

        public ProductoViewModel()
        {
            ProductoItems = new ObservableCollection<ProductoItemViewModel>();

            Dictionary<string, int> list = new Dictionary<string, int>();
            list.Add("id0", 0);
            //Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;
            LoadProducto(list, 1, 30);


            SearchCommand = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("Hpña", "buscando", "Cancelar");
            });
        }

        private async void LoadProducto(Dictionary<string, int> dictionary, int page, int items)
        {
            try
            {
                IsRefreshing = true;
                // www.lineatienda.com/services.php/productos/categoria/1/100
                Dictionary<string, int>[] dataSend = { dictionary };

                RootObject<Producto> rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto>>("productos", String.Format("categoria/{0}/{1}", page, items), dataSend);
                foreach (Producto item in rootData.datos)
                {
                    ProductoItems.Add(new ProductoItemViewModel()
                    {
                        nombreProducto = item.nombreProducto,
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
