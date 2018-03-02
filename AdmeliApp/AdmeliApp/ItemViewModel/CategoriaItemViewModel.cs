using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ItemViewModel
{
    public class CategoriaItemViewModel
    {
        public int idCategoria { get; set; }
        public string nombreCategoria { get; set; }
        public int idPadreCategoria { get; set; }
        public string padre { get; set; }
        public int ordenVisualizacionProductos { get; set; }
        public int mostrarProductosEn { get; set; }
        public int numeroColumnas { get; set; }
        public string tituloCategoriaSeo { get; set; }
        public string urlCategoriaSeo { get; set; }
        public string metaTagsSeo { get; set; }
        public string cabeceraPagina { get; set; }
        public string piePagina { get; set; }
        public int orden { get; set; }
        public int estado { get; set; }
        public int mostrarWeb { get; set; }
        public string tieneRegistros { get; set; }
        public bool relacionPrincipal { get; set; }
        public bool afecta { get; set; }


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
