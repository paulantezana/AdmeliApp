﻿using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class UnidadMedida : BaseModel
    {
        public int idUnidadMedida { get; set; }
        public string nombreUnidad { get; set; }
        public string simbolo { get; set; }
        public int estado { get; set; }
        public string tieneRegistros { get; set; }

    }
}
