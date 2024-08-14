using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szerver
{
    class Meghivas
    {
        string felhasznalo1;
        string felhasznalo2;
        string targy;
        string statusz;

        public Meghivas(string felhasznalo1, string felhasznalo2, string targy, string statusz)
        {
            Felhasznalo1 = felhasznalo1;
            Felhasznalo2 = felhasznalo2;
            Targy = targy;
            Statusz = statusz;
        }

        public string Felhasznalo1 { get => felhasznalo1; set => felhasznalo1 = value; }
        public string Felhasznalo2 { get => felhasznalo2; set => felhasznalo2 = value; }
        public string Targy { get => targy; set => targy = value; }
        public string Statusz { get => statusz; set => statusz = value; }
    }
}
