using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class ProductoViewModel : BaseViewModelPagination
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
            this.LoadRegisters();
        }

        public ICommand SearchCommand { get; set; }

        public ProductoViewModel()
        {
            ProductoItems = new ObservableCollection<ProductoItemViewModel>();
            this.LoadRegisters();
            SearchCommand = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("Hpña", "buscando", "Cancelar");
            });
        }

        public override async void LoadRegisters()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/productos/categoria/1/100
                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);

                Dictionary<string, int>[] dataSend = { list };

                RootObject<Producto> rootData = await webService.POST<Dictionary<string, int>[], RootObject<Producto>>("productos", String.Format("categoria/{0}/{1}", paginacion.currentPage, App.configuracionGeneral.itemPorPagina), dataSend);


                // Set data paginacion
                this.paginacion.itemsCount = rootData.nro_registros;
                this.paginacion.reload();

                // Reload pagination
                this.reloadPagination();

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
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                this.IsRefreshing = false;
                this.IsEnabled = true;
            }
        }
    }
}
