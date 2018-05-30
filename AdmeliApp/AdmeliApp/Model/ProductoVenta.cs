using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class ProductoVenta : BaseModel
    {
        public int idProducto { get; set; }
        public string codigoProducto { get; set; }
        public string nombreProducto { get; set; }
        public string precioVenta { get; set; }
        public int idPresentacion { get; set; }
        public bool ventaVarianteSinStock { get; set; }
        public string nombreMarca { get; set; }
    }
}
