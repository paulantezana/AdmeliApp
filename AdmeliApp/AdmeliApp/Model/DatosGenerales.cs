using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class DatosGenerales : BaseModel
    {
        public int idDatosGenerales { get; set; }
        public string razonSocial { get; set; }
        public string ruc { get; set; }
        public string direccion { get; set; }
        public string logoEmpresa { get; set; }
        public string email { get; set; }
        public string cuentaBancaria { get; set; }
        public int estado { get; set; }
        public int idUbicacionGeografica { get; set; }
    }
}
