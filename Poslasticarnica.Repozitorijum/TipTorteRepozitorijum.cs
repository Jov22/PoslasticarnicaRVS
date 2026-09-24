using Microsoft.Data.SqlClient;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class TipTorteRepozitorijum : Tabela
    {
        public List<TipTorte> VratiSve()
        {
            List<TipTorte> lista = new List<TipTorte>();

            using (SqlConnection konekcija = KreirajKonekciju())
            {
                string upit =
                    "SELECT TipTorteID, Naziv, Cena FROM TipTorte";

                SqlCommand komanda =
                    KreirajKomandu(upit, konekcija);

                konekcija.Open();

                using (SqlDataReader citac = komanda.ExecuteReader())
                {
                    while (citac.Read())
                    {
                        TipTorte tip = new TipTorte
                        {
                            TipTorteID =
                                Convert.ToInt32(citac["TipTorteID"]),

                            Naziv =
                                citac["Naziv"].ToString() ?? "",

                            Cena =
                                Convert.ToDecimal(citac["Cena"])
                        };

                        lista.Add(tip);
                    }
                }
            }

            return lista;
        }

        public TipTorte? VratiPoId(int id)
        {
            TipTorte? tip = null;

            using (SqlConnection konekcija = KreirajKonekciju())
            {
                string upit =
                    @"SELECT TipTorteID, Naziv, Cena
                      FROM TipTorte
                      WHERE TipTorteID = @TipTorteID";

                SqlCommand komanda =
                    KreirajKomandu(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", id);

                konekcija.Open();

                using (SqlDataReader citac = komanda.ExecuteReader())
                {
                    if (citac.Read())
                    {
                        tip = new TipTorte
                        {
                            TipTorteID =
                                Convert.ToInt32(citac["TipTorteID"]),

                            Naziv =
                                citac["Naziv"].ToString() ?? "",

                            Cena =
                                Convert.ToDecimal(citac["Cena"])
                        };
                    }
                }
            }

            return tip;
        }

        public void Dodaj(TipTorte tip)
        {
            using (SqlConnection konekcija = KreirajKonekciju())
            {
                string upit =
                    @"INSERT INTO TipTorte (Naziv, Cena)
                      VALUES (@Naziv, @Cena)";

                SqlCommand komanda =
                    KreirajKomandu(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@Naziv", tip.Naziv);

                komanda.Parameters.AddWithValue(
                    "@Cena", tip.Cena);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Izmeni(TipTorte tip)
        {
            using (SqlConnection konekcija = KreirajKonekciju())
            {
                string upit =
                    @"UPDATE TipTorte
                      SET Naziv = @Naziv,
                          Cena = @Cena
                      WHERE TipTorteID = @TipTorteID";

                SqlCommand komanda =
                    KreirajKomandu(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@Naziv", tip.Naziv);

                komanda.Parameters.AddWithValue(
                    "@Cena", tip.Cena);

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", tip.TipTorteID);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Obrisi(int id)
        {
            using (SqlConnection konekcija = KreirajKonekciju())
            {
                string upit =
                    @"DELETE FROM TipTorte
                      WHERE TipTorteID = @TipTorteID";

                SqlCommand komanda =
                    KreirajKomandu(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", id);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }
    }
}