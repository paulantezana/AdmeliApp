using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class ConfiguracionGeneral
    {
        public int idConfiguracionGeneral { get; set; }
        public int numeroDecimales { get; set; }
        public int itemPorPagina { get; set; }
        public string porcentajeUtilidad { get; set; }
        public string imagenPorDefecto { get; set; }
        public string logoImpresion { get; set; }
        public int idDatosGenerales { get; set; }
        public int estado { get; set; }
        public bool arquearMarcador { get; set; }
    }
}
