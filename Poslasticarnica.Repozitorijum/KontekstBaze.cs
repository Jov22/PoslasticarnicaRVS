using Microsoft.EntityFrameworkCore;
using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class KontekstBaze : DbContext
    {
        public DbSet<Proizvod> Proizvodi { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder opcije)
        {
            opcije.UseSqlServer(
                Konekcija.VratiKonekcioniString());
        }

        protected override void OnModelCreating(
            ModelBuilder graditeljModela)
        {
            graditeljModela.Entity<Proizvod>()
                .ToTable("Proizvod");

            graditeljModela.Entity<Proizvod>()
                .HasKey(p => p.ProizvodID);
        }
    }
}