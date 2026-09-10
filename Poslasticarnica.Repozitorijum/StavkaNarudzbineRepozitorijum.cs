using Microsoft.Data.SqlClient;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class StavkaNarudzbineRepozitorijum
    {
        public List<StavkaNarudzbine> VratiZaNarudzbinu(
            int narudzbinaID)
        {
            List<StavkaNarudzbine> lista =
                new List<StavkaNarudzbine>();

            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT
                        s.StavkaNarudzbineID,
                        s.NarudzbinaID,
                        s.ProizvodID,
                        s.Kolicina,
                        p.Naziv AS NazivProizvoda,
                        p.Cena AS CenaPoKomadu
                    FROM StavkaNarudzbine s
                    INNER JOIN Proizvod p
                        ON s.ProizvodID = p.ProizvodID
                    WHERE s.NarudzbinaID = @NarudzbinaID
                    ORDER BY s.StavkaNarudzbineID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@NarudzbinaID",
                    narudzbinaID);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(
                        NapraviStavku(reader));
                }
            }

            return lista;
        }

        public StavkaNarudzbine? VratiPoId(int id)
        {
            StavkaNarudzbine? stavka = null;

            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT
                        s.StavkaNarudzbineID,
                        s.NarudzbinaID,
                        s.ProizvodID,
                        s.Kolicina,
                        p.Naziv AS NazivProizvoda,
                        p.Cena AS CenaPoKomadu
                    FROM StavkaNarudzbine s
                    INNER JOIN Proizvod p
                        ON s.ProizvodID = p.ProizvodID
                    WHERE s.StavkaNarudzbineID =
                          @StavkaNarudzbineID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@StavkaNarudzbineID",
                    id);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                if (reader.Read())
                {
                    stavka =
                        NapraviStavku(reader);
                }
            }

            return stavka;
        }

        public void Dodaj(
            StavkaNarudzbine stavka)
        {
            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    INSERT INTO StavkaNarudzbine
                    (
                        NarudzbinaID,
                        ProizvodID,
                        Kolicina
                    )
                    VALUES
                    (
                        @NarudzbinaID,
                        @ProizvodID,
                        @Kolicina
                    )";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@NarudzbinaID",
                    stavka.NarudzbinaID);

                komanda.Parameters.AddWithValue(
                    "@ProizvodID",
                    stavka.ProizvodID);

                komanda.Parameters.AddWithValue(
                    "@Kolicina",
                    stavka.Kolicina);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Izmeni(
            StavkaNarudzbine stavka)
        {
            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    UPDATE StavkaNarudzbine
                    SET
                        ProizvodID = @ProizvodID,
                        Kolicina = @Kolicina
                    WHERE StavkaNarudzbineID =
                          @StavkaNarudzbineID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@ProizvodID",
                    stavka.ProizvodID);

                komanda.Parameters.AddWithValue(
                    "@Kolicina",
                    stavka.Kolicina);

                komanda.Parameters.AddWithValue(
                    "@StavkaNarudzbineID",
                    stavka.StavkaNarudzbineID);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Obrisi(int id)
        {
            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    DELETE FROM StavkaNarudzbine
                    WHERE StavkaNarudzbineID =
                          @StavkaNarudzbineID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@StavkaNarudzbineID",
                    id);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public decimal IzracunajOsnovnuCenu(
            int narudzbinaID)
        {
            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT
                        ISNULL(
                            SUM(p.Cena * s.Kolicina),
                            0
                        )
                    FROM StavkaNarudzbine s
                    INNER JOIN Proizvod p
                        ON s.ProizvodID = p.ProizvodID
                    WHERE s.NarudzbinaID =
                          @NarudzbinaID";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@NarudzbinaID",
                    narudzbinaID);

                konekcija.Open();

                object? rezultat =
                    komanda.ExecuteScalar();

                if (rezultat == null ||
                    rezultat == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToDecimal(
                    rezultat);
            }
        }

        private StavkaNarudzbine NapraviStavku(
            SqlDataReader reader)
        {
            return new StavkaNarudzbine
            {
                StavkaNarudzbineID =
                    Convert.ToInt32(
                        reader["StavkaNarudzbineID"]),

                NarudzbinaID =
                    Convert.ToInt32(
                        reader["NarudzbinaID"]),

                ProizvodID =
                    Convert.ToInt32(
                        reader["ProizvodID"]),

                Kolicina =
                    Convert.ToInt32(
                        reader["Kolicina"]),

                NazivProizvoda =
                    reader["NazivProizvoda"]
                        .ToString() ?? "",

                CenaPoKomadu =
                    Convert.ToDecimal(
                        reader["CenaPoKomadu"])
            };
        }
    }
}