using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class PuntoDeVenta
    {
        public int idPuntoVenta { get; set; }
        public string nombre { get; set; }
        public bool ventaWeb { get; set; }
        public int estado { get; set; }
        public int idSucursal { get; set; }
        public string sucursal { get; set; }

        public int idAsignarPuntoVenta { get; set; }
    }
}
