using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Model
{
    public class Fecha
    {
        public DateTime date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
        public override string ToString()
        {
            return date.ToString();
        }
    }
}
