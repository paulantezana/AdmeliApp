using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Cotizacion : BaseModel
    {
        public int idCotizacion { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }
        public string nombreCliente { get; set; }
        public string direccion { get; set; }
        public string rucDni { get; set; }
        public int idGrupoCliente { get; set; }
        public string moneda { get; set; }
        public string descuento { get; set; }
        public string subTotal { get; set; }
        public string total { get; set; }
        public string tipoCambio { get; set; }
        public int idMoneda { get; set; }
        public Fecha fechaEmision { get; set; }
        public Fecha fechaVencimiento { get; set; }
        public string observacion { get; set; }
        public int estado { get; set; }
        public int idCliente { get; set; }
        public int idSucursal { get; set; }
        public string personal { get; set; }
        public string documentoIdentificacion { get; set; }
    }
}
