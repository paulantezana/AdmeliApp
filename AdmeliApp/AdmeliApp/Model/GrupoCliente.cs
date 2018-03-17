using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class GrupoCliente : BaseModel
    {
        public int idGrupoCliente { get; set; }
        public string nombreGrupo { get; set; }
        public string descripcion { get; set; }
        public int minimoOrden { get; set; }
        public int estado { get; set; }
        public bool enUso { get; set; }

        [JsonIgnore]
        public Color BackgroundItem { get; set; }

        [JsonIgnore]
        public Color TextColorItem { get; set; }
    }
}
