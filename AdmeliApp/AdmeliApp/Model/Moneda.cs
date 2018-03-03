using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Moneda : BaseModel
    {
        public int idMoneda { get; set; }
        public int idMonedaPorDefecto { get; set; }

        public string moneda { get; set; }
        public string simbolo { get; set; }
        public bool porDefecto { get; set; }
        public int estado { get; set; }
        public string tipoCambio { get; set; }
        public Fecha fechaCreacion { get; set; }
        public dynamic idPersonal { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string tieneRegistros { get; set; }

        public string fechaPago { get; set; }
        public int idCaja { get; set; }
        public int idCajaSesion { get; set; }
        public int idMedioPago { get; set; }
        public string medioPago { get; set; }
        public double monto { get; set; }
        public string motivo { get; set; }
        public string numeroOperacion { get; set; }
        public string observacion { get; set; }

        public double total { get; set; }
    }
}
