using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Venta : BaseModel
    {
        public Fecha fecha { get; set; }
        public int idVenta { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }
        public string nombreCliente { get; set; }
        public string rucDni { get; set; }
        public string direccion { get; set; }
        public int idGrupoCliente { get; set; }
        public string formaPago { get; set; }
        public Fecha fechaPago { get; set; }
        public Fecha fechaVenta { get; set; }
        public string observacion { get; set; }
        public string subTotal { get; set; }
        public string total { get; set; }
        public string descuento { get; set; }
        public string tipoVenta { get; set; }
        public string moneda { get; set; }
        public string tipoCambio { get; set; }
        public int estado { get; set; }
        public int idCliente { get; set; }
        public int idCobro { get; set; }
        public int idAsignarPuntoVenta { get; set; }
        public int idTipoDocumento { get; set; }
        public string nombreLabel { get; set; }
        public string notaSalida { get; set; }
        public string vendedor { get; set; }
        public string guiaRemision { get; set; }
        public string documentoIdentificacion { get; set; }

        public string numeroDocumento { get; set; }
    }
}
