using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class Sucursal : BaseModel
    {
        public int idSucursal { get; set; }
        public string nombre { get; set; }
        public bool principal { get; set; }
        public int estado { get; set; }
        public string estados { get; set; }
        public string direccion { get; set; }
        public int idUbicacionGeografica { get; set; }
        public string tieneRegistros { get; set; }
    }
}
