using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class AlternativaCombinacion
    {
        public int idCombinacionAlternativa { get; set; }
        public int idCombinacionAlternativaStockAlmacen { get; set; }
        public int idProducto { get; set; }
        public int idAlmacen { get; set; }
        public string alertaStock { get; set; }
        public string alternativas { get; set; }
        public string codigoSku { get; set; }
        public string nombreCombinacion { get; set; }
        public string precio { get; set; }
        public string stock { get; set; }
        public string stockIdeal { get; set; }
        public string stockMinimo { get; set; }
    }

    public class CombinacionStock
    {
        public int idCombinacionAlternativa { get; set; }
        public string alternativas { get; set; }
        public double precio { get; set; }
        public string nombreCombinacion { get; set; }
        public int idPresentacion { get; set; }
        public int idProducto { get; set; }
        public string codigoSku { get; set; }
        public string stock { get; set; }
        public int idAlmacen { get; set; }
        public string productoAlmacen { get; set; }

        public double precioVenta { get; set; }

        public string precioVentaTotal;
        public string PrecioVentaTotal
        {
            get
            {
                return (precio + precioVenta).ToString();
            }
            set
            {
                precioVentaTotal = value;
            }
        }
    }
}
