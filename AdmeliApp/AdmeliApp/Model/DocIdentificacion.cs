using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class DocIdentificacion : BaseModel
    {
        public int idDocumento { get; set; }
        public string nombre { get; set; }
        public int numeroDigitos { get; set; }
        public string tipoDocumento { get; set; }
        public int estado { get; set; }
    }
}
