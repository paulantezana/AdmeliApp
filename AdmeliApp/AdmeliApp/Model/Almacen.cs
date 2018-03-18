using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class Almacen : BaseModel
    {
        public int idAlmacen { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public bool principal { get; set; }
        public int estado { get; set; }
        public int idSucursal { get; set; }
        public int idUbicacionGeografica { get; set; }
        public string nombreSucursal { get; set; }
        public string tieneRegistros { get; set; }

        public int idPersonalAlmacen { get; set; }
    }
}
