using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private WebService webService = new WebService();

        public ObservableCollection<CategoriaViewModel> CategoriaItems { get; set; }
        public ObservableCollection<UnidadMedidaViewModel> UnidadMedidaItems { get; set; }
        public ObservableCollection<MarcaViewModel> MarcaItems { get; set; }

        #region ================================== List Refreshing ==================================
        private bool isRefreshingCategoria { get; set; }
        public bool IsRefreshingCategoria
        {
            set
            {
                if (isRefreshingCategoria != value)
                {
                    isRefreshingCategoria = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingCategoria"));
                }
            }
            get
            {
                return isRefreshingCategoria;
            }
        }

        private bool isRefreshingUnidadMedida { get; set; }
        public bool IsRefreshingUnidadMedida
        {
            set
            {
                if (isRefreshingUnidadMedida != value)
                {
                    isRefreshingUnidadMedida = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingUnidadMedida"));
                }
            }
            get
            {
                return isRefreshingUnidadMedida;
            }
        }

        private bool isRefreshingMarca { get; set; }
        public bool IsRefreshingMarca
        {
            set
            {
                if (isRefreshingMarca != value)
                {
                    isRefreshingMarca = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingMarca"));
                }
            }
            get
            {
                return isRefreshingMarca;
            }
        }
        #endregion



        public ICommand RefreshCategoriaCommand { get; private set; }
        public ICommand RefreshUnidadMedidaCommand { get; private set; }
        public ICommand RefreshMarcaCommand { get; private set; }

        public MainViewModel()
        {
            CategoriaItems = new ObservableCollection<CategoriaViewModel>();
            LoadCategoria();

            UnidadMedidaItems = new ObservableCollection<UnidadMedidaViewModel>();
            LoadUnidadMedida(1,30);

            MarcaItems = new ObservableCollection<MarcaViewModel>();
            LoadMarca(1,30);


            RefreshCategoriaCommand = new Command(() =>
            {
                CategoriaItems.Clear();
                LoadCategoria();
            });


            RefreshUnidadMedidaCommand = new Command(() =>
            {
                UnidadMedidaItems.Clear();
                LoadUnidadMedida(1,30);
            });

            RefreshMarcaCommand = new Command(() =>
            {
                MarcaItems.Clear();
                LoadMarca(1,30);
            });
        }

        private async void LoadCategoria()
        {
            try
            {
                IsRefreshingCategoria = true;
                // www.lineatienda.com/services.php/categoriastree
                RootObject<CategoriaViewModel> rootData = await webService.GET<RootObject<CategoriaViewModel>>("categoriastree");
                foreach (CategoriaViewModel item in rootData.datos)
                {
                    CategoriaItems.Add(new CategoriaViewModel()
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
                IsRefreshingCategoria = false;
            }
        }

        private async void LoadMarca(int page, int items)
        {
            try
            {
                IsRefreshingMarca = true;
                // www.lineatienda.com/services.php/marcas/estado/1/100
                RootObject<MarcaViewModel> rootData = await webService.GET<RootObject<MarcaViewModel>>("marcas", String.Format("estado/{0}/{1}", page, items));
                foreach (MarcaViewModel item in rootData.datos)
                {
                    MarcaItems.Add(new MarcaViewModel()
                    {
                        nombreMarca = item.nombreMarca,
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsRefreshingMarca = false;
            }
        }

        private async void LoadUnidadMedida(int page, int items)
        {
            try
            {
                IsRefreshingUnidadMedida = true;
                // www.lineatienda.com/services.php/unimedidas/estado/1/100
                RootObject<UnidadMedidaViewModel> rootData = await webService.GET<RootObject<UnidadMedidaViewModel>>("unimedidas", String.Format("estado/{0}/{1}", page, items));
                foreach (UnidadMedidaViewModel item in rootData.datos)
                {
                    UnidadMedidaItems.Add(new UnidadMedidaViewModel()
                    {
                        nombreUnidad = item.nombreUnidad,
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsRefreshingUnidadMedida = false;
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
