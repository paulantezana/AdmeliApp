using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Producto
    {
        public int idProducto { get; set; }
        public bool cantidadFraccion { get; set; }
        public string codigoBarras { get; set; }
        public string codigoProducto { get; set; }
        public string controlSinStock { get; set; }
        public string descripcionCorta { get; set; }
        public string descripcionLarga { get; set; }
        public bool enCategoriaEstrella { get; set; }
        public bool enPortada { get; set; }
        public bool enUso { get; set; }
        public bool estado { get; set; }
        public int idMarca { get; set; }
        public int idUnidadMedida { get; set; }
        public string keywords { get; set; }
        public string limiteMaximo { get; set; }
        public string limiteMinimo { get; set; }
        public bool mostrarPrecioWeb { get; set; }
        public bool mostrarVideo { get; set; }
        public bool mostrarWeb { get; set; }
        public string nombreMarca { get; set; }
        public string nombreProducto { get; set; }
        public string nombreUnidad { get; set; }
        public dynamic precioCompra { get; set; }
        public string urlVideo { get; set; }
        public bool ventaVarianteSinStock { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
    }
}
