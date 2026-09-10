using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslasticarnica.Repozitorijum
{
    public static class Konekcija
    {
        public static string VratiKonekcioniString()
        {
            return @"Server=DESKTOP-77NFMHQ\SQLEXPRESS;Database=PoslasticarnicaDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }
    }
}