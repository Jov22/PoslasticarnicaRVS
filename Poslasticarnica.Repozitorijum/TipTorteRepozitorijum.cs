using Microsoft.Data.SqlClient;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class TipTorteRepozitorijum
    {
        public List<TipTorte> VratiSve()
        {
            List<TipTorte> lista = new List<TipTorte>();

            using (SqlConnection konekcija =
                new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit =
                    "SELECT TipTorteID, Naziv FROM TipTorte";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                while (reader.Read())
                {
                    TipTorte tip = new TipTorte
                    {
                        TipTorteID =
                            Convert.ToInt32(reader["TipTorteID"]),

                        Naziv =
                            reader["Naziv"].ToString() ?? ""
                    };

                    lista.Add(tip);
                }
            }

            return lista;
        }

        public TipTorte? VratiPoId(int id)
        {
            TipTorte? tip = null;

            using (SqlConnection konekcija =
                new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit =
                    @"SELECT TipTorteID, Naziv
                      FROM TipTorte
                      WHERE TipTorteID = @TipTorteID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", id);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                if (reader.Read())
                {
                    tip = new TipTorte
                    {
                        TipTorteID =
                            Convert.ToInt32(reader["TipTorteID"]),

                        Naziv =
                            reader["Naziv"].ToString() ?? ""
                    };
                }
            }

            return tip;
        }

        public void Dodaj(TipTorte tip)
        {
            using (SqlConnection konekcija =
                new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit =
                    @"INSERT INTO TipTorte (Naziv)
                      VALUES (@Naziv)";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@Naziv", tip.Naziv);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Izmeni(TipTorte tip)
        {
            using (SqlConnection konekcija =
                new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit =
                    @"UPDATE TipTorte
                      SET Naziv = @Naziv
                      WHERE TipTorteID = @TipTorteID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@Naziv", tip.Naziv);

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", tip.TipTorteID);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Obrisi(int id)
        {
            using (SqlConnection konekcija =
                new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit =
                    @"DELETE FROM TipTorte
                      WHERE TipTorteID = @TipTorteID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", id);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }
    }
}