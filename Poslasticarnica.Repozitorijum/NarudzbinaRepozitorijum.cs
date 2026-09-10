using Microsoft.Data.SqlClient;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class NarudzbinaRepozitorijum
    {
        public List<Narudzbina> VratiSve()
        {
            List<Narudzbina> lista = new List<Narudzbina>();

            using (SqlConnection konekcija =
                   new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit = @"SELECT 
                                NarudzbinaID,
                                ImePrezimeKupca,
                                BrojTelefona,
                                Email,
                                DatumKreiranja,
                                DatumPreuzimanja,
                                Status,
                                UkupnaCena,
                                Napomena,
                                TipTorteID
                                FROM Narudzbina";

                SqlCommand komanda = new SqlCommand(upit, konekcija);

                konekcija.Open();

                SqlDataReader reader = komanda.ExecuteReader();

                while (reader.Read())
                {
                    Narudzbina narudzbina = new Narudzbina
                    {
                        NarudzbinaID = Convert.ToInt32(reader["NarudzbinaID"]),
                        ImePrezimeKupca = reader["ImePrezimeKupca"].ToString(),
                        BrojTelefona = reader["BrojTelefona"].ToString(),
                        Email = reader["Email"].ToString(),
                        DatumKreiranja = Convert.ToDateTime(reader["DatumKreiranja"]),
                        DatumPreuzimanja = Convert.ToDateTime(reader["DatumPreuzimanja"]),
                        Status = reader["Status"].ToString(),
                        UkupnaCena = Convert.ToDecimal(reader["UkupnaCena"]),
                        Napomena = reader["Napomena"].ToString(),
                        TipTorteID = Convert.ToInt32(reader["TipTorteID"])
                    };

                    lista.Add(narudzbina);
                }
            }

            return lista;
        }

        public Narudzbina VratiPoId(int id)
        {
            Narudzbina narudzbina = null;

            using (SqlConnection konekcija =
                   new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit = @"SELECT 
                                NarudzbinaID,
                                ImePrezimeKupca,
                                BrojTelefona,
                                Email,
                                DatumKreiranja,
                                DatumPreuzimanja,
                                Status,
                                UkupnaCena,
                                Napomena,
                                TipTorteID
                                FROM Narudzbina
                                WHERE NarudzbinaID = @NarudzbinaID";

                SqlCommand komanda = new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@NarudzbinaID", id);

                konekcija.Open();

                SqlDataReader reader = komanda.ExecuteReader();

                if (reader.Read())
                {
                    narudzbina = new Narudzbina
                    {
                        NarudzbinaID = Convert.ToInt32(reader["NarudzbinaID"]),
                        ImePrezimeKupca = reader["ImePrezimeKupca"].ToString(),
                        BrojTelefona = reader["BrojTelefona"].ToString(),
                        Email = reader["Email"].ToString(),
                        DatumKreiranja = Convert.ToDateTime(reader["DatumKreiranja"]),
                        DatumPreuzimanja = Convert.ToDateTime(reader["DatumPreuzimanja"]),
                        Status = reader["Status"].ToString(),
                        UkupnaCena = Convert.ToDecimal(reader["UkupnaCena"]),
                        Napomena = reader["Napomena"].ToString(),
                        TipTorteID = Convert.ToInt32(reader["TipTorteID"])
                    };
                }
            }

            return narudzbina;
        }

        public void Dodaj(Narudzbina narudzbina)
        {
            using (SqlConnection konekcija =
                   new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit = @"INSERT INTO Narudzbina
                                (
                                    ImePrezimeKupca,
                                    BrojTelefona,
                                    Email,
                                    DatumKreiranja,
                                    DatumPreuzimanja,
                                    Status,
                                    UkupnaCena,
                                    Napomena,
                                    TipTorteID
                                )
                                VALUES
                                (
                                    @ImePrezimeKupca,
                                    @BrojTelefona,
                                    @Email,
                                    @DatumKreiranja,
                                    @DatumPreuzimanja,
                                    @Status,
                                    @UkupnaCena,
                                    @Napomena,
                                    @TipTorteID
                                )";

                SqlCommand komanda = new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@ImePrezimeKupca", narudzbina.ImePrezimeKupca);

                komanda.Parameters.AddWithValue(
                    "@BrojTelefona", narudzbina.BrojTelefona);

                komanda.Parameters.AddWithValue(
                    "@Email", narudzbina.Email ?? "");

                komanda.Parameters.AddWithValue(
                    "@DatumKreiranja", narudzbina.DatumKreiranja);

                komanda.Parameters.AddWithValue(
                    "@DatumPreuzimanja", narudzbina.DatumPreuzimanja);

                komanda.Parameters.AddWithValue(
                    "@Status", narudzbina.Status);

                komanda.Parameters.AddWithValue(
                    "@UkupnaCena", narudzbina.UkupnaCena);

                komanda.Parameters.AddWithValue(
                    "@Napomena", narudzbina.Napomena ?? "");

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", narudzbina.TipTorteID);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }

        public void Izmeni(Narudzbina narudzbina)
        {
            using (SqlConnection konekcija =
                   new SqlConnection(Konekcija.VratiKonekcioniString()))
            {
                string upit = @"UPDATE Narudzbina
                                SET
                                    ImePrezimeKupca = @ImePrezimeKupca,
                                    BrojTelefona = @BrojTelefona,
                                    Email = @Email,
                                    DatumKreiranja = @DatumKreiranja,
                                    DatumPreuzimanja = @DatumPreuzimanja,
                                    Status = @Status,
                                    UkupnaCena = @UkupnaCena,
                                    Napomena = @Napomena,
                                    TipTorteID = @TipTorteID
                                WHERE NarudzbinaID = @NarudzbinaID";

                SqlCommand komanda = new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@NarudzbinaID", narudzbina.NarudzbinaID);

                komanda.Parameters.AddWithValue(
                    "@ImePrezimeKupca", narudzbina.ImePrezimeKupca);

                komanda.Parameters.AddWithValue(
                    "@BrojTelefona", narudzbina.BrojTelefona);

                komanda.Parameters.AddWithValue(
                    "@Email", narudzbina.Email ?? "");

                komanda.Parameters.AddWithValue(
                    "@DatumKreiranja", narudzbina.DatumKreiranja);

                komanda.Parameters.AddWithValue(
                    "@DatumPreuzimanja", narudzbina.DatumPreuzimanja);

                komanda.Parameters.AddWithValue(
                    "@Status", narudzbina.Status);

                komanda.Parameters.AddWithValue(
                    "@UkupnaCena", narudzbina.UkupnaCena);

                komanda.Parameters.AddWithValue(
    "@Napomena",
    narudzbina.Napomena ?? "");

                komanda.Parameters.AddWithValue(
                    "@TipTorteID", narudzbina.TipTorteID);

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
                    "DELETE FROM Narudzbina WHERE NarudzbinaID = @NarudzbinaID";

                SqlCommand komanda = new SqlCommand(upit, konekcija);

                komanda.Parameters.AddWithValue(
                    "@NarudzbinaID", id);

                konekcija.Open();

                komanda.ExecuteNonQuery();
            }
        }
    }
}