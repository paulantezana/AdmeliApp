using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Personal
    {
        public int idPersonal { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public Fecha fechaNacimiento { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string sexo { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string direccion { get; set; }
        public int estado { get; set; }
        public int idUbicacionGeografica { get; set; }
        public int idDocumento { get; set; }
    }
}
