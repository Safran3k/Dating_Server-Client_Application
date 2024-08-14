using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szerver
{
    class Felhasznalo
    {
        string felhasznalonev;
        string jelszo;

        public Felhasznalo(string felhasznalonev, string jelszo)
        {
            Felhasznalonev = felhasznalonev;
            Jelszo = jelszo;
        }

        public string Felhasznalonev { get => felhasznalonev; set => felhasznalonev = value; }
        public string Jelszo { get => jelszo; set => jelszo = value; }
    }
}
