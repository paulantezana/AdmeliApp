using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class Impuesto : BaseModel
    {
        public int idImpuesto { get; set; }
        public string nombreImpuesto { get; set; }
        public string siglasImpuesto { get; set; }
        public string valorImpuesto { get; set; }
        public bool porcentual { get; set; }
        public bool porDefecto { get; set; }
        public int estado { get; set; }
        public bool enUso { get; set; }

        [JsonIgnore]
        public Color BackgroundItem { get; set; }

        [JsonIgnore]
        public Color TextColorItem { get; set; }
    }
}
