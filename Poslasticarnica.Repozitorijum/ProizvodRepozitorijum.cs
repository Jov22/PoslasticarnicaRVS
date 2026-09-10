using Microsoft.Data.SqlClient;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class ProizvodRepozitorijum
    {
        public List<Proizvod> VratiSve()
        {
            List<Proizvod> lista =
                new List<Proizvod>();

            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT
                        ProizvodID,
                        Naziv,
                        Kategorija,
                        Cena
                    FROM Proizvod
                    ORDER BY Kategorija, Naziv";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                while (reader.Read())
                {
                    Proizvod proizvod =
                        new Proizvod
                        {
                            ProizvodID =
                                Convert.ToInt32(
                                    reader["ProizvodID"]),

                            Naziv =
                                reader["Naziv"]
                                    .ToString() ?? "",

                            Kategorija =
                                reader["Kategorija"]
                                    .ToString() ?? "",

                            Cena =
                                Convert.ToDecimal(
                                    reader["Cena"])
                        };

                    lista.Add(proizvod);
                }
            }

            return lista;
        }

        public Proizvod? VratiPoId(int id)
        {
            Proizvod? proizvod = null;

            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT
                        ProizvodID,
                        Naziv,
                        Kategorija,
                        Cena
                    FROM Proizvod
                    WHERE ProizvodID = @ProizvodID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@ProizvodID",
                    id);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                if (reader.Read())
                {
                    proizvod =
                        new Proizvod
                        {
                            ProizvodID =
                                Convert.ToInt32(
                                    reader["ProizvodID"]),

                            Naziv =
                                reader["Naziv"]
                                    .ToString() ?? "",

                            Kategorija =
                                reader["Kategorija"]
                                    .ToString() ?? "",

                            Cena =
                                Convert.ToDecimal(
                                    reader["Cena"])
                        };
                }
            }

            return proizvod;
        }
    }
}