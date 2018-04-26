using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class OrdenCompra : BaseModel
    {
        public int idOrdenCompra { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }
        public string nombreProveedor { get; set; }
        public string rucDni { get; set; }
        public string direccionProveedor { get; set; }
        public string moneda { get; set; }
        public Fecha fecha { get; set; }
        public Fecha plazoEntrega { get; set; }
        public string observacion { get; set; }
        public string direccion { get; set; }
        public int estado { get; set; }
        public int idUbicacionGeografica { get; set; }
        public int idTipoDocumento { get; set; }
        public int idCompra { get; set; }
        public string subTotal { get; set; }
        public string total { get; set; }
        public int idPago { get; set; }
        public int idProveedor { get; set; }
        public int estadoCompra { get; set; }
        public string nombres { get; set; }

        private string estadoString;
        public string EstadoString
        {
            get
            {
                if (estado == 1) { return "Activo"; }
                else { return "Anulado"; }
            }
            set
            {
                estadoString = value;
            }
        }
    }
}
