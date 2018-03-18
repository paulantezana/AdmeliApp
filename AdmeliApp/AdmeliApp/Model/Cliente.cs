using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Cliente : BaseModel
    {
        public int idCliente { get; set; }
        public string nombreCliente { get; set; }
        public string numeroDocumento { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string sexo { get; set; }
        public string direccion { get; set; }
        public string zipCode { get; set; }
        public bool esEventual { get; set; }
        public string observacion { get; set; }
        public int estado { get; set; }
        public int idUbicacionGeografica { get; set; }
        public int idGrupoCliente { get; set; }
        public string nombreGrupo { get; set; }
        public string nroVentasCotizaciones { get; set; }
        public int idDocumento { get; set; }
        public string nombre { get; set; }
        public string tipoDocumento { get; set; }
    }
}
