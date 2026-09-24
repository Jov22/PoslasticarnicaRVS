using Microsoft.Data.SqlClient;

namespace Poslasticarnica.Repozitorijum
{
    public class Tabela
    {
        protected string KonekcioniString { get; }

        public Tabela()
        {
            KonekcioniString = Konekcija.VratiKonekcioniString();
        }

        protected SqlConnection KreirajKonekciju()
        {
            return new SqlConnection(KonekcioniString);
        }

        protected SqlCommand KreirajKomandu(string upit, SqlConnection konekcija)
        {
            return new SqlCommand(upit, konekcija);
        }
    }
}