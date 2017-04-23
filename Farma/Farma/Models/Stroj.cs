using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farma.Models
{
    public class Stroj
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Svrha { get; set; }
        public string GodisnjeDoba { get; set; }
        public bool Upotreba { get; set; }
        public string Korisnik { get; set; }
    }
}