using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Marca
    {
        [JsonProperty(PropertyName = "idMarca")]
        public int idMarca { get; set; }

        [JsonProperty(PropertyName = "nombreMarca")]
        public string NombreMarca { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty(PropertyName = "sitioWeb")]
        public string SitioWeb { get; set; }

        [JsonProperty(PropertyName = "ubicacionLogo")]
        public string UbicacionLogo { get; set; }

        [JsonProperty(PropertyName = "captionImagen")]
        public string CaptionImagen { get; set; }

        [JsonProperty(PropertyName = "estado")]
        public int Estado { get; set; }

        [JsonProperty(PropertyName = "tieneRegistros")]
        public string TieneRegistros { get; set; }
    }
}
