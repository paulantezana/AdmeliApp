using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AdmeliApp.ViewModel
{
    public class UnidadMedidaViewModel
    {
        internal WebService webService = new WebService();

        public ObservableCollection<UnidadMedidaViewModel> UnidadMedidaItems { get; set; }

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

        public ICommand RefreshUnidadMedidaCommand { get; private set; }

        public UnidadMedidaMainViewModel()
        {
            UnidadMedidaItems = new ObservableCollection<UnidadMedidaViewModel>();
            LoadUnidadMedida(1, 30);

            RefreshUnidadMedidaCommand = new Command(() =>
            {
                UnidadMedidaItems.Clear();
                LoadUnidadMedida(1, 30);
            });
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
