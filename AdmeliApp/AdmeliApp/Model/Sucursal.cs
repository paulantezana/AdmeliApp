using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Sucursal
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
