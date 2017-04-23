using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Farma.Models
{
    public class Stoka
    {
        public int ID { get; set;}
        public string Vrsta { get; set; }
        public int Tezina { get; set; }
        public int Starost { get; set; }
        public string Boja { get; set; }
        public string Nadimak { get; set; }
    }
}