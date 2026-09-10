using Microsoft.Data.SqlClient;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class KorisnikRepozitorijum
    {
        public Korisnik? Prijava(
            string korisnickoIme,
            string lozinka)
        {
            Korisnik? korisnik = null;

            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT KorisnikID,
                           KorisnickoIme,
                           Lozinka,
                           Uloga
                    FROM Korisnik
                    WHERE KorisnickoIme = @KorisnickoIme
                    AND Lozinka = @Lozinka";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@KorisnickoIme",
                    korisnickoIme);

                komanda.Parameters.AddWithValue(
                    "@Lozinka",
                    lozinka);

                konekcija.Open();

                SqlDataReader reader =
                    komanda.ExecuteReader();

                if (reader.Read())
                {
                    korisnik = new Korisnik
                    {
                        KorisnikID =
                            Convert.ToInt32(
                                reader["KorisnikID"]),

                        KorisnickoIme =
                            reader["KorisnickoIme"]
                                .ToString() ?? "",

                        Lozinka =
                            reader["Lozinka"]
                                .ToString() ?? "",

                        Uloga =
                            reader["Uloga"]
                                .ToString() ?? ""
                    };
                }
            }

            return korisnik;
        }

        public bool PostojiKorisnickoIme(
            string korisnickoIme)
        {
            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    SELECT COUNT(*)
                    FROM Korisnik
                    WHERE KorisnickoIme = @KorisnickoIme";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@KorisnickoIme",
                    korisnickoIme);

                konekcija.Open();

                int broj =
                    Convert.ToInt32(
                        komanda.ExecuteScalar());

                return broj > 0;
            }
        }

        public void Registruj(Korisnik korisnik)
        {
            using (SqlConnection konekcija =
                new SqlConnection(
                    Konekcija.VratiKonekcioniString()))
            {
                string upit = @"
                    INSERT INTO Korisnik
                    (
                        KorisnickoIme,
                        Lozinka,
                        Uloga
                    )
                    VALUES
                    (
                        @KorisnickoIme,
                        @Lozinka,
                        @Uloga
                    )";

                SqlCommand komanda =
                    new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@KorisnickoIme",
                    korisnik.KorisnickoIme);

                komanda.Parameters.AddWithValue(
                    "@Lozinka",
                    korisnik.Lozinka);

                komanda.Parameters.AddWithValue(
                    "@Uloga",
                    korisnik.Uloga);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }
    }
}