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
    public class CategoriaMainViewModel : INotifyPropertyChanged
    {
        internal WebService webService = new WebService();

        public ObservableCollection<CategoriaViewModel> CategoriaItems { get; set; }

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


        public ICommand RefreshCategoriaCommand { get; private set; }

        public CategoriaMainViewModel()
        {
            CategoriaItems = new ObservableCollection<CategoriaViewModel>();
            LoadCategoria();

            RefreshCategoriaCommand = new Command(() =>
            {
                CategoriaItems.Clear();
                LoadCategoria();
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
