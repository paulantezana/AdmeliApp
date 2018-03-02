using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.ItemViewModel
{
    public class MarcaItemViewModel
    {
        public int idMarca { get; set; }
        public string nombreMarca { get; set; }
        public string descripcion { get; set; }
        public string sitioWeb { get; set; }
        public string ubicacionLogo { get; set; }
        public string captionImagen { get; set; }
        public int estado { get; set; }
        public string tieneRegistros { get; set; }
    }
}
