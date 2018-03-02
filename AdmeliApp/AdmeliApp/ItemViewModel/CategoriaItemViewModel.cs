using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ItemViewModel
{
    public class CategoriaItemViewModel : Categoria
    {



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
