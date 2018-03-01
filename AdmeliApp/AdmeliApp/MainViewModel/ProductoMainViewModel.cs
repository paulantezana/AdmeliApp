using AdmeliApp.Helpers;
using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.MainViewModel
{
    public class ProductoMainViewModel : INotifyPropertyChanged
    {
        internal WebService webService = new WebService();

        public ObservableCollection<ProductoViewModel> ProductoItems { get; set; }

        public string SearchText { get; set; }

        private bool isRefreshingProducto { get; set; }
        public bool IsRefreshingProducto
        {
            set
            {
                if (isRefreshingProducto != value)
                {
                    isRefreshingProducto = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingProducto"));
                }
            }
            get
            {
                return isRefreshingProducto;
            }
        }

        public ICommand RefreshProductoCommand { get; private set; }

        public ICommand SearchCommand { get; set; }

        public ProductoMainViewModel()
        {
            ProductoItems = new ObservableCollection<ProductoViewModel>();

            Dictionary<string, int> list = new Dictionary<string, int>();
            list.Add("id0", 0);
            //Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;
            LoadProducto(list, 1, 30);

            RefreshProductoCommand = new Command(() =>
            {
                ProductoItems.Clear();
                Dictionary<string, int> listReSend = new Dictionary<string, int>();
                listReSend.Add("id0", 0);
                //Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;
                LoadProducto(list, 1, 30);
            });


            SearchCommand = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("Hpña", "buscando", "Cancelar");
            });
        }

        private async void LoadProducto(Dictionary<string, int> dictionary, int page, int items)
        {
            try
            {
                IsRefreshingProducto = true;
                // www.lineatienda.com/services.php/productos/categoria/1/100
                Dictionary<string, int>[] dataSend = { dictionary };

                RootObject<ProductoViewModel> rootData = await webService.POST<Dictionary<string, int>[], RootObject<ProductoViewModel>>("productos", String.Format("categoria/{0}/{1}", page, items), dataSend);
                foreach (ProductoViewModel item in rootData.datos)
                {
                    ProductoItems.Add(new ProductoViewModel()
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
                IsRefreshingProducto = false;
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
