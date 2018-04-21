using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model.Location
{
    public class UbicacionGeografica
    {
        public int idUbicacionGeografica { get; set; }
        public int idPais { get; set; }
        public int idNivel1 { get; set; }
        public int idNivel2 { get; set; }
        public int idNivel3 { get; set; }
        public string nombreP { get; set; }
        public string nombreN1 { get; set; }
        public string nombreN2 { get; set; }
        public string nombreN3 { get; set; }
    }

    public class UbicacionGeograficaG // datos para guardar datos de UbicacionGeografica
    {
        public int idPais { get; set; }
        public int idNivel1 { get; set; }
        public int idNivel2 { get; set; }
        public int idNivel3 { get; set; }
    }
}
