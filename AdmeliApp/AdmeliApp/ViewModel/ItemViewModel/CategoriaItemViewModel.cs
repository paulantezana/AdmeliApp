using AdmeliApp.Helpers;
using AdmeliApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class CategoriaItemViewModel : Categoria
    {
        internal WebService webService = new WebService();
        public bool Nuevo;

        private bool _DeleteIsEnabled;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool DeleteIsEnabled
        {
            get { return _DeleteIsEnabled; }
            set { SetValue(ref _DeleteIsEnabled, value); }
        }

        public ICommand Editar { get; private set; }

        public CategoriaItemViewModel()
        {
            Editar = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar a: ? ", "Aceptar", "Cancelar");
            });
        }
    }
}
