using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class TipoDocumento : BaseModel
    {
        public int idTipoDocumento { get; set; }
        public string nombre { get; set; }
        public string nombreLabel { get; set; }
        public string descripcion { get; set; }
        public bool comprobante { get; set; }
        public string area { get; set; }
        public int tipoCliente { get; set; }
        public int estado { get; set; }

        public string formatoDocumento { get; set; }
        public bool redimensionarModelo { get; set; }
        public bool bordeDetalle { get; set; }

        [JsonIgnore]
        public Color BackgroundItem { get; set; }

        [JsonIgnore]
        public Color TextColorItem { get; set; }
    }
}
