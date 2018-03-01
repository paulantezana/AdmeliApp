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
    public class MarcaMainViewModel : INotifyPropertyChanged
    {
        internal WebService webService = new WebService();

        public ObservableCollection<MarcaViewModel> MarcaItems { get; set; }

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

        public ICommand RefreshMarcaCommand { get; private set; }

        public MarcaMainViewModel()
        {
            MarcaItems = new ObservableCollection<MarcaViewModel>();
            LoadMarca(1, 30);

            RefreshMarcaCommand = new Command(() =>
            {
                MarcaItems.Clear();
                LoadMarca(1, 30);
            });
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
