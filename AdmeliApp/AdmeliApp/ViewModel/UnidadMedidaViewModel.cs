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
    public class UnidadMedidaViewModel : BaseViewModel
    {
        internal WebService webService = new WebService();

        public ObservableCollection<UnidadMedidaItemViewModel> UnidadMedidaItems { get; set; }

        private ICommand refreshUnidadMedidaCommand;
        public ICommand RefreshUnidadMedidaCommand =>
            refreshUnidadMedidaCommand ?? (refreshUnidadMedidaCommand = new Command(() => ExecuteRefreshUnidadMedidaAsync()));

        private void ExecuteRefreshUnidadMedidaAsync()
        {
            UnidadMedidaItems.Clear();
            LoadUnidadMedida(1, 30);
        }

        public UnidadMedidaViewModel()
        {
            UnidadMedidaItems = new ObservableCollection<UnidadMedidaItemViewModel>();
            LoadUnidadMedida(1, 30);
        }

        private async void LoadUnidadMedida(int page, int items)
        {
            try
            {
                IsRefreshing = true;
                // www.lineatienda.com/services.php/unimedidas/estado/1/100
                RootObject<UnidadMedida> rootData = await webService.GET<RootObject<UnidadMedida>>("unimedidas", String.Format("estado/{0}/{1}", page, items));
                foreach (UnidadMedida item in rootData.datos)
                {
                    UnidadMedidaItems.Add(new UnidadMedidaItemViewModel()
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
                IsRefreshing = false;
            }
        }
    }
}
