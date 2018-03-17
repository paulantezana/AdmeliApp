using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class CajaSesion : BaseModel
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public int idCajaSesion { get; set; }
        public Fecha fechaInicio { get; set; }
        public Fecha fechaCierre { get; set; }
        public int estado { get; set; }
        public string totalIngreso { get; set; }
        public string totalEgreso { get; set; }
        public string nombre { get; set; }

        [JsonIgnore]
        public Color BackgroundItem { get; set; }

        [JsonIgnore]
        public Color TextColorItem { get; set; }
    }
}
