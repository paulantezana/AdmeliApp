using AdmeliApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class TipoCambioMoneda : BaseModel
    {
        public string moneda { get; set; }
        public int cambio { get; set; }
    }
}
