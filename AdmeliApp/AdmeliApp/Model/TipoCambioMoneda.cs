using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class TipoCambioMoneda : BaseModel
    {
        public string moneda { get; set; }
        public int cambio { get; set; }
    }
}
