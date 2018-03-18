using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class Denominacion : BaseModel
    {
        public int idDenominacion { get; set; }
        public string tipoMoneda { get; set; }
        public string nombre { get; set; }
        public string valor { get; set; }
        public string imagen { get; set; }
        public int estado { get; set; }
        public int idMoneda { get; set; }
        public string moneda { get; set; }
        public string anular { get; set; }
    }
}
