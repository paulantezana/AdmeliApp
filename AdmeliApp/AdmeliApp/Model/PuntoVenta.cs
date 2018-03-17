using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class PuntoVenta : BaseModel
    {
        public int idPuntoVenta { get; set; }
        public string nombre { get; set; }
        public bool ventaWeb { get; set; }
        public int estado { get; set; }
        public int idSucursal { get; set; }
        public string sucursal { get; set; }

        public int idAsignarPuntoVenta { get; set; }

        [JsonIgnore]
        public Color BackgroundItem { get; set; }

        [JsonIgnore]
        public Color TextColorItem { get; set; }
    }
}
