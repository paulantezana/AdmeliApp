using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Descuento : BaseModel
    {
        public int idDescuentoProductoGrupo { get; set; }
        public string codigo { get; set; }
        public string descuento { get; set; }
        public Fecha fechaInicio { get; set; }
        public Fecha fechaFin { get; set; }
        public string tipoDescuento { get; set; }
        public string tipo { get; set; }
        public string cantidadMinima { get; set; }
        public string cantidadMaxima { get; set; }
        public int estado { get; set; }
        public int idGrupoCliente { get; set; }
        public string nombreGrupo { get; set; }
        public int idSucursal { get; set; }
        public string nombre { get; set; }
        public int idAfectoProducto { get; set; }

        public int idProducto { get; set; }
        public string nombreSucursal { get; set; }
        public object nombreProducto { get; set; }
    }
}
